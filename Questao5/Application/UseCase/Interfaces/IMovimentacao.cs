using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Application.UseCase.Movimentacoes.ViewModel;

namespace Questao5.Application.UseCase.Interfaces
{
    public interface IMovimentacao
    {
        Task<MovimentacaoViewModel> MovimentarContaAsync(MovimentacaoInputModel inputModel);
    }
}
