using Questao5.Domain.Entities;

namespace Questao5.Domain.Repository
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetByKeyAsync(string chaveIdempotencia);
        Task AddAsync(Idempotencia idempotencia);
    }
}
