
namespace Questao5.Application.UseCase.Saldos.ViewModel
{
    public class SaldoViewModel
    {
        public string NumeroContaCorrente { get; internal set; }
        public string NomeTitular { get; internal set; }
        public DateTime DataHoraConsulta { get; internal set; }
        public decimal SaldoAtual { get; internal set; }
    }
}
