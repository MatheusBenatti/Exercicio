using NSubstitute;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;

using Xunit;

namespace Questao5.Test.Infrastructure.Database
{
    public class MovimentoRepositoryTests
    {
        private readonly IMovimentoRepository _mockRepository;

        public MovimentoRepositoryTests()
        {
            _mockRepository = Substitute.For<IMovimentoRepository>();
        }

        [Fact]
        public async Task GetSomaCreditosAsync_ReturnsCorrectSum_WhenCreditsExist()
        {
            #region Arrange
            var idContaCorrente = "345435345";
            _mockRepository.GetSomaCreditosAsync(idContaCorrente).Returns(Task.FromResult(250.00m));
            #endregion

            #region Act
            var result = await _mockRepository.GetSomaCreditosAsync(idContaCorrente);
            #endregion

            #region Assert
            Assert.Equal(250.00m, result);
            await _mockRepository.Received(1).GetSomaCreditosAsync(idContaCorrente);
            #endregion
        }

        [Fact]
        public async Task GetSomaCreditosAsync_ReturnsZero_WhenNoCreditsExist()
        {
            #region Arrange
            var idContaCorrente = "345435345";
            _mockRepository.GetSomaCreditosAsync(idContaCorrente).Returns(Task.FromResult(0m));
            #endregion

            #region Act
            var result = await _mockRepository.GetSomaCreditosAsync(idContaCorrente);
            #endregion

            #region Assert
            Assert.Equal(0m, result);
            await _mockRepository.Received(1).GetSomaCreditosAsync(idContaCorrente);
            #endregion
        }

        [Fact]
        public async Task GetSomaDebitosAsync_ReturnsCorrectSum_WhenDebitsExist()
        {
            #region Arrange
            var idContaCorrente = "345435345";
            _mockRepository.GetSomaDebitosAsync(idContaCorrente).Returns(Task.FromResult(125.00m));
            #endregion

            #region Act
            var result = await _mockRepository.GetSomaDebitosAsync(idContaCorrente);
            #endregion

            #region Assert
            Assert.Equal(125.00m, result);
            await _mockRepository.Received(1).GetSomaDebitosAsync(idContaCorrente);
            #endregion
        }

        [Fact]
        public async Task GetSomaDebitosAsync_ReturnsZero_WhenNoDebitsExist()
        {
            #region Arrange
            var idContaCorrente = "345435345";
            _mockRepository.GetSomaDebitosAsync(idContaCorrente).Returns(Task.FromResult(0m));
            #endregion

            #region Act
            var result = await _mockRepository.GetSomaDebitosAsync(idContaCorrente);
            #endregion

            #region Assert
            Assert.Equal(0m, result);
            await _mockRepository.Received(1).GetSomaDebitosAsync(idContaCorrente);
            #endregion
        }

        [Fact]
        public async Task AddAsync_AddsMovimento_WhenCalled()
        {
            #region Arrange
            var movimento = new Movimento
            {
                IdMovimento = "3",
                IdContaCorrente = "345435345",
                DataMovimento = DateTime.Now,
                TipoMovimento = "C",
                Valor = 200.00m
            };
            #endregion

            #region Act
            await _mockRepository.AddAsync(movimento);
            #endregion

            #region Assert
            await _mockRepository.Received(1).AddAsync(movimento);
            #endregion
        }
    }
}
