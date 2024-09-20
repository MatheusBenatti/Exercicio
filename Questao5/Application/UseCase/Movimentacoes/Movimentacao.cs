using Questao5.Application.UseCase.Interfaces;
using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Application.UseCase.Movimentacoes.ViewModel;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using System.Net;

namespace Questao5.Application.UseCase.Movimentacoes
{
    public class Movimentacao : IMovimentacao
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public Movimentacao(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentoRepository movimentoRepository,
            IIdempotenciaRepository idempotenciaRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<MovimentacaoViewModel> MovimentarContaAsync(MovimentacaoInputModel requestInput)
        {
            var idempotencia = await _idempotenciaRepository.GetByKeyAsync(requestInput.IdRequisicao);
            if (idempotencia != null)
            {
                return new MovimentacaoViewModel
                {
                    IdMovimento = idempotencia.Resultado
                };
            }

            var conta = await _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                ?? throw new Exception("Conta corrente não cadastrada.")
                { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INVALID_ACCOUNT" } } };

            if (conta.Ativo == 1)
            {
                throw new Exception("Conta corrente inativa.")
                { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INACTIVE_ACCOUNT" } } };
            }

            if (requestInput.Valor <= 0)
            {
                throw new Exception("Valor inválido.")
                { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INVALID_VALUE" } } };
            }

            if (requestInput.TipoMovimento != "C" && requestInput.TipoMovimento != "D")
            {
                throw new Exception("Tipo de movimento inválido.")
                { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INVALID_TYPE" } } };
            }

            var movimento = new Movimento
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = requestInput.IdContaCorrente,
                DataMovimento = DateTime.UtcNow,
                TipoMovimento = requestInput.TipoMovimento,
                Valor = requestInput.Valor
            };

            await _movimentoRepository.AddAsync(movimento);

            await _idempotenciaRepository.AddAsync(new Idempotencia
            {
                ChaveIdempotencia = requestInput.IdRequisicao,
                Requisicao = $"Movimentação de {requestInput.Valor} em conta {requestInput.IdContaCorrente}",
                Resultado = movimento.IdMovimento
            });

            return new MovimentacaoViewModel
            {
                IdMovimento = movimento.IdMovimento
            };
        }
    }

}
