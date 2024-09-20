using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Questao5.Application.UseCase.Interfaces;
using Questao5.Application.UseCase.Saldos.InputModel;
using Questao5.Application.UseCase.Saldos.ViewModel;
using Questao5.Infrastructure.Services.ApiModel;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.Test.Infrastructure
{
    public class SaldoControllerTests
    {
        private readonly SaldoController _controller;
        private readonly ISaldo _saldoUseCase;

        public SaldoControllerTests()
        {
            _saldoUseCase = Substitute.For<ISaldo>();
            _controller = new SaldoController(_saldoUseCase);
        }

        [Fact]
        public async Task ConsultarSaldo_ReturnsOk_WhenSaldoIsFound()
        {
            #region Arrange
            var requestApi = new SaldoApiModel
            {
                IdContaCorrente = "12345"
            };

            var expectedResponse = new SaldoViewModel
            {
                NumeroContaCorrente = "12345",
                NomeTitular = "John Doe",
                DataHoraConsulta = System.DateTime.UtcNow,
                SaldoAtual = 5000
            };

            _saldoUseCase.ConsultarSaldoAsync(Arg.Any<SaldoInputModel>())
                .Returns(await Task.FromResult(expectedResponse));
            #endregion

            #region Act
            var result = await _controller.ConsultarSaldo(requestApi);
            #endregion

            #region Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
            #endregion
        }

        [Fact]
        public async Task ConsultarSaldo_ReturnsBadRequest_WhenExceptionOccurs()
        {
            #region Arrange
            var requestApi = new SaldoApiModel
            {
                IdContaCorrente = "12345"
            };

            var exception = new Exception("Conta não encontrada.");
            exception.Data["Tipo"] = "INVALID_ACCOUNT";

            _saldoUseCase.ConsultarSaldoAsync(Arg.Any<SaldoInputModel>())
                .Throws(exception);
            #endregion

            #region Act
            var result = await _controller.ConsultarSaldo(requestApi);
            #endregion

            #region Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequestResult.Value;

            response.Should().BeEquivalentTo(new
            {
                Mensagem = "Conta não encontrada.",
                Tipo = "INVALID_ACCOUNT"
            });
            #endregion
        }
    }
}
