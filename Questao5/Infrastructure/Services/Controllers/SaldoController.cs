using Microsoft.AspNetCore.Mvc;
using Questao5.Application.UseCase.Interfaces;
using Questao5.Application.UseCase.Saldos.InputModel;
using Questao5.Infrastructure.Services.ApiModel;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly ISaldo _saldoUseCase;

        public SaldoController(ISaldo saldoUseCase)
        {
            _saldoUseCase = saldoUseCase;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarSaldo([FromQuery] SaldoApiModel request)
        {
            SaldoInputModel saldo = new()
            {
                IdContaCorrente = request.IdContaCorrente,
            };

            try
            {
                var saldoResponse = await _saldoUseCase.ConsultarSaldoAsync(saldo);
                return Ok(saldoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message, Tipo = ex.Data["Tipo"] });
            }
        }
    }
}
