using Questao5.Domain.Entities;

namespace Questao5.Domain.Repository
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetByIdAsync(string idContaCorrente);
    }
}
