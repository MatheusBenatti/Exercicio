namespace Questao5.Application.UseCase.Movimentacoes.InputModel
{
    public class MovimentacaoInputModel
    {
        public string IdRequisicao { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}
