using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Infrastructure.Services.ApiModel;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Questao5.Application.UseCase.Movimentacoes.ViewModel;
using Questao5.Application.UseCase.Interfaces;

namespace Questao5.Test.Infrastructure
{
    public class MovimentacaoControllerTests
    {
        private readonly MovimentacaoController _controller;
        private readonly IMovimentacao _movimentacaoUseCase;

        public MovimentacaoControllerTests()
        {
            _movimentacaoUseCase = Substitute.For<IMovimentacao>();
            _controller = new MovimentacaoController(_movimentacaoUseCase);
        }

        [Fact]
        public async Task MovimentarConta_ReturnsOk_WhenMovimentacaoSucceeds()
        {
            #region Arrange
            var requestApi = new MovimentacaoApiModel
            {
                IdContaCorrente = "12345",
                IdRequisicao = "REQ001",
                TipoMovimento = "C",
                Valor = 100
            };

            var expectedResponse = new MovimentacaoViewModel
            {
                IdMovimento = "MOV001"
            };

            _movimentacaoUseCase.MovimentarContaAsync(Arg.Any<MovimentacaoInputModel>())
                .Returns(await Task.FromResult(expectedResponse));
            #endregion

            #region Act
            var result = await _controller.MovimentarConta(requestApi);
            #endregion

            #region Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
            #endregion
        }

        [Fact]
        public async Task MovimentarConta_ReturnsBadRequest_WhenExceptionOccurs()
        {
            #region Arrange
            var requestApi = new MovimentacaoApiModel
            {
                IdContaCorrente = "12345",
                IdRequisicao = "REQ001",
                TipoMovimento = "D",
                Valor = 50
            };

            var exception = new Exception("Invalid Account");
            exception.Data["Tipo"] = "INVALID_ACCOUNT";

            _movimentacaoUseCase.MovimentarContaAsync(Arg.Any<MovimentacaoInputModel>())
                .Throws(exception);
            #endregion

            #region Act
            var result = await _controller.MovimentarConta(requestApi);
            #endregion

            #region Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequestResult.Value;

            response.Should().BeEquivalentTo(new
            {
                Mensagem = "Invalid Account",
                Tipo = "INVALID_ACCOUNT"
            });
            #endregion
        }
    }
}
