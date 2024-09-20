using Questao5.Application.UseCase.Saldos.InputModel;
using Questao5.Application.UseCase.Saldos.ViewModel;
using Questao5.Domain.Repository;
using System.Net;

namespace Questao5.Application.UseCase.Saldos
{
    public class Saldo
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public Saldo(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<SaldoViewModel> ConsultarSaldoAsync(SaldoInputModel request)
        {
            var conta = await _contaCorrenteRepository.GetByIdAsync(request.IdContaCorrente) ?? 
                throw new Exception("Conta corrente não cadastrada.") { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INVALID_ACCOUNT" } } };
            
            if (conta.Ativo == 1)
            {
                throw new Exception("Conta corrente inativa.") { HResult = (int)HttpStatusCode.BadRequest, Data = { { "Tipo", "INACTIVE_ACCOUNT" } } };
            }

            var creditos = await _movimentoRepository.GetSomaCreditosAsync(request.IdContaCorrente);
            var debitos = await _movimentoRepository.GetSomaDebitosAsync(request.IdContaCorrente);

            var saldo = creditos - debitos;

            return new SaldoViewModel
            { 
                NumeroContaCorrente = conta.IdContaCorrente,
                NomeTitular = conta.Nome,
                DataHoraConsulta = DateTime.UtcNow,
                SaldoAtual = saldo
            };
        }
    }
}
