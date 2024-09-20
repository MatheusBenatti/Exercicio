using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Questao5.Infrastructure.Services.ApiModel
{
    public class SaldoApiModel
    {
        [Required]
        [JsonPropertyName("id_conta_corrente")]
        public string IdContaCorrente { get; set; }
    }
}
