using Questao5.Application.UseCase.Saldos.InputModel;
using Questao5.Application.UseCase.Saldos.ViewModel;

namespace Questao5.Application.UseCase.Interfaces
{
    public interface ISaldo
    {
        Task<SaldoViewModel> ConsultarSaldoAsync(SaldoInputModel inputModel);
    }
}
