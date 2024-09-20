using NSubstitute;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using Xunit;

namespace Questao5.Test.Infrastructure.Database
{
    public class IdempotenciaRepositoryTests
    {
        private readonly IIdempotenciaRepository _mockRepository;

        public IdempotenciaRepositoryTests()
        {
            _mockRepository = Substitute.For<IIdempotenciaRepository>();
        }

        [Fact]
        public async Task GetByKeyAsync_ReturnsIdempotencia_WhenExists()
        {
            #region Arrange
            var chaveIdempotencia = "unique_key";
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = chaveIdempotencia,
                Requisicao = "request_data",
                Resultado = "result_data"
            };

            _mockRepository.GetByKeyAsync(chaveIdempotencia).Returns(idempotencia);
            #endregion

            #region Act
            var result = await _mockRepository.GetByKeyAsync(chaveIdempotencia);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(chaveIdempotencia, result.ChaveIdempotencia);
            Assert.Equal("request_data", result.Requisicao);
            Assert.Equal("result_data", result.Resultado);
            #endregion
        }

        [Fact]
        public async Task GetByKeyAsync_ReturnsNull_WhenNotExists()
        {
            #region Arrange
            _mockRepository.GetByKeyAsync("nonexistent_key").Returns((Idempotencia)null);
            #endregion

            #region Act
            var result = await _mockRepository.GetByKeyAsync("nonexistent_key");
            #endregion

            #region Assert
            Assert.Null(result);
            #endregion
        }

        [Fact]
        public async Task AddAsync_CallsAddMethod_WhenCalled()
        {
            #region Arrange
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = "unique_key",
                Requisicao = "request_data",
                Resultado = "result_data"
            };
            #endregion

            #region Act
            await _mockRepository.AddAsync(idempotencia);
            #endregion

            #region Assert
            await _mockRepository.Received(1).AddAsync(idempotencia);
            #endregion
        }
    }
}
