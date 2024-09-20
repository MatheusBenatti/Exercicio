using NSubstitute;
using Questao5.Application.UseCase.Saldos.InputModel;
using Questao5.Application.UseCase.Saldos;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using System.Net;
using Xunit;

namespace Questao5.Test.Application
{
    public class SaldoTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentoRepository _movimentoRepository = Substitute.For<IMovimentoRepository>();

        private readonly Saldo _saldoUseCase;

        public SaldoTests()
        {
            _saldoUseCase = new Saldo(_contaCorrenteRepository, _movimentoRepository);
        }

        [Fact]
        public async Task ConsultarSaldo_ThrowsException_WhenContaCorrenteIsNotFound()
        {
            #region Arrange
            var requestInput = new SaldoInputModel
            {
                IdContaCorrente = "12345"
            };

            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult<ContaCorrente>(null!));
            #endregion

            #region Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _saldoUseCase.ConsultarSaldoAsync(requestInput));
            Assert.Equal("Conta corrente não cadastrada.", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HResult);
            Assert.Equal("INVALID_ACCOUNT", exception.Data["Tipo"]);
            #endregion
        }

        [Fact]
        public async Task ConsultarSaldo_ThrowsException_WhenContaCorrenteIsInactive()
        {
            #region Arrange
            var requestInput = new SaldoInputModel
            {
                IdContaCorrente = "12345"
            };

            var contaCorrente = new ContaCorrente { IdContaCorrente = "12345", Ativo = 1 };
            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(contaCorrente));
            #endregion

            #region Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _saldoUseCase.ConsultarSaldoAsync(requestInput));
            Assert.Equal("Conta corrente inativa.", exception.Message);
            Assert.Equal((int)HttpStatusCode.BadRequest, exception.HResult);
            Assert.Equal("INACTIVE_ACCOUNT", exception.Data["Tipo"]);
            #endregion
        }

        [Fact]
        public async Task ConsultarSaldo_ReturnsSaldoViewModel_WhenContaIsValid()
        {
            #region Arrange
            var requestInput = new SaldoInputModel
            {
                IdContaCorrente = "12345"
            };

            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = "12345",
                Nome = "John Doe",
                Ativo = 0
            };

            _contaCorrenteRepository.GetByIdAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(contaCorrente));

            _movimentoRepository.GetSomaCreditosAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(500m));

            _movimentoRepository.GetSomaDebitosAsync(requestInput.IdContaCorrente)
                .Returns(Task.FromResult(200m));
            #endregion

            #region Act
            var result = await _saldoUseCase.ConsultarSaldoAsync(requestInput);
            #endregion

            #region Assert
            Assert.Equal("12345", result.NumeroContaCorrente);
            Assert.Equal("John Doe", result.NomeTitular);
            Assert.True(result.SaldoAtual == 300m); 
            Assert.True((DateTime.UtcNow - result.DataHoraConsulta).TotalSeconds < 1);
            #endregion
        }
    }
}
