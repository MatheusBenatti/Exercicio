using NSubstitute;
using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Application.UseCase.Movimentacoes;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using System.Net;
using Xunit;

namespace Questao5.Test.Application
{
    public class MovimentacaoTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentoRepository _movimentoRepository = Substitute.For<IMovimentoRepository>();
        private readonly IIdempotenciaRepository _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();

        private readonly Movimentacao _movimentacaoUseCase;

        public MovimentacaoTests()
        {
            _movimentacaoUseCase = new Movimentacao(
                _contaCorrenteRepository,
                _movimentoRepository,
                _idempotenciaRepository
            );
        }

        [Fact]
        public async Task MovimentarConta_ThrowsException_WhenContaCorrenteIsNotFound()
        {
            #region Arrange
            var requestInput = new MovimentacaoInputModel
            {
                IdContaCorrente = "12345",
                IdRequisicao = "REQ001",
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult<ContaCorrente>(null!));
            #endregion

            #region Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _movimentacaoUseCase.MovimentarContaAsync(requestInput));
            Assert.Equal("Conta corrente não cadastrada.", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HResult);
            Assert.Equal("INVALID_ACCOUNT", exception.Data["Tipo"]);
            #endregion
        }

        [Fact]
        public async Task MovimentarConta_ThrowsException_WhenContaCorrenteIsInactive()
        {
            #region Arrange
            var requestInput = new MovimentacaoInputModel
            {
                IdContaCorrente = "12345",
                IdRequisicao = "REQ001",
                TipoMovimento = "C",
                Valor = 100
            };
            
            var contaCorrente = new ContaCorrente { IdContaCorrente = "12345", Ativo = 1 };
            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(contaCorrente));
            #endregion

            #region Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _movimentacaoUseCase.MovimentarContaAsync(requestInput));
            Assert.Equal("Conta corrente inativa.", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HResult);
            Assert.Equal("INACTIVE_ACCOUNT", exception.Data["Tipo"]);
            #endregion
        }

        [Fact]
        public async Task MovimentarConta_ThrowsException_WhenValorIsInvalid()
        {
            #region Arrange
            var requestInput = new MovimentacaoInputModel
            {
                IdContaCorrente = "12345",
                IdRequisicao = "REQ001",
                TipoMovimento = "C",
                Valor = -50
            };

            var contaCorrente = new ContaCorrente { IdContaCorrente = "12345", Ativo = 0 };
            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(contaCorrente));
            #endregion

            #region Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _movimentacaoUseCase.MovimentarContaAsync(requestInput));
            Assert.Equal("Valor inválido.", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HResult);
            Assert.Equal("INVALID_VALUE", exception.Data["Tipo"]);
            #endregion
        }
    }
}
