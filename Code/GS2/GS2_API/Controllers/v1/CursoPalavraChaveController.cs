using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [Route("api/curso-palavra-chave")]
    [ApiController]
    public class CursoPalavraChaveController : ControllerBase
    {
        private readonly CursoPalavraChaveService _service;

        public CursoPalavraChaveController(CursoPalavraChaveService service)
        {
            _service = service;
        }

        [HttpGet("curso/{cursoId:int}")]
        public async Task<IActionResult> ListarPorCurso(int cursoId)
        {
            var result = await _service.ListarPorCursoAsync(cursoId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] CursoPalavraChaveDto dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");

            var result = await _service.AdicionarAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{cursoId:int}/{palavraChaveId:int}")]
        public async Task<IActionResult> Remover(int cursoId, int palavraChaveId)
        {
            await _service.RemoverAsync(cursoId, palavraChaveId);
            return NoContent();
        }
    }
}
