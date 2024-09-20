using NSubstitute;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Xunit;

namespace Questao5.Test.Infrastructure.Database
{
    public class ContaCorrenteRepositoryTests
    {
        private readonly IContaCorrenteRepository _mockRepository;

        public ContaCorrenteRepositoryTests()
        {
            _mockRepository = Substitute.For<IContaCorrenteRepository>();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsContaCorrente_WhenExists()
        {
            #region Arrange
            var contaCorrente = new ContaCorrente
            {
                IdContaCorrente = "345435345",
                Nome = "John Doe"
            };

            _mockRepository.GetByIdAsync("345435345").Returns(contaCorrente);
            #endregion

            #region Act
            var result = await _mockRepository.GetByIdAsync("345435345");
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal("345435345", result.IdContaCorrente);
            Assert.Equal("John Doe", result.Nome);
            #endregion
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            #region Arrange
            _mockRepository.GetByIdAsync("nonexistent").Returns((ContaCorrente)null);
            #endregion

            #region Act
            var result = await _mockRepository.GetByIdAsync("nonexistent");
            #endregion

            #region Assert
            Assert.Null(result);
            #endregion
        }
    }
}
