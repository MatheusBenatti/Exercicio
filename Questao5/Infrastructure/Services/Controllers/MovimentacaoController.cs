using Microsoft.AspNetCore.Mvc;
using Questao5.Application.UseCase.Movimentacoes;
using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Infrastructure.Services.ApiModel;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly Movimentacao _movimentacaoService;

        public MovimentacaoController(Movimentacao movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoApiModel requestApi)
        {
            MovimentacaoInputModel requestInput = new()
            {
                IdContaCorrente = requestApi.IdContaCorrente,
                IdRequisicao = requestApi.IdRequisicao,
                TipoMovimento = requestApi.TipoMovimento,
                Valor = requestApi.Valor
            };
            try
            {
                var response = await _movimentacaoService.MovimentarContaAsync(requestInput);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message, Tipo = ex.Data["Tipo"] });
            }
        }
    }
}
