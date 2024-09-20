using Questao5.Domain.Entities;

namespace Questao5.Domain.Repository
{
    public interface IMovimentoRepository
    {
        Task<decimal> GetSomaCreditosAsync(string idContaCorrente);
        Task<decimal> GetSomaDebitosAsync(string idContaCorrente);
        Task AddAsync(Movimento movimento);
    }
}
