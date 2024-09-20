using Microsoft.AspNetCore.Mvc;
using Questao5.Application.UseCase.Interfaces;
using Questao5.Application.UseCase.Movimentacoes.InputModel;
using Questao5.Infrastructure.Services.ApiModel;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacao _movimentacaoUseCase;

        public MovimentacaoController(IMovimentacao movimentacaoUseCase)
        {
            _movimentacaoUseCase = movimentacaoUseCase;
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
                var response = await _movimentacaoUseCase.MovimentarContaAsync(requestInput);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message, Tipo = ex.Data["Tipo"] });
            }
        }
    }
}
