using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Questao5.Infrastructure.Services.ApiModel
{
    public class MovimentacaoApiModel
    {
        [Required]
        [JsonPropertyName("id_requisicao")]
        public string IdRequisicao { get; set; }
        [Required]
        [JsonPropertyName("Id_conta_corrente")]
        public string IdContaCorrente { get; set; }
        [Required]
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
        [Required]
        [RegularExpression("C|D")]
        [JsonPropertyName("tipo_movimento")]
        public string TipoMovimento { get; set; }
    }
}
