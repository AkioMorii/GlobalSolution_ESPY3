using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [Route("api/curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly CursoService _service;

        public CursoController(CursoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obter(int id)
        {
            var result = await _service.ObterPorIdAsync(id);
            if (result == null)
                return NotFound($"Curso com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CursoDto request)
        {
            if (request == null)
                return BadRequest("Payload inválido.");

            var result = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(Obter), new { id = result.CursoId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CursoDto request)
        {
            if (request == null)
                return BadRequest("Payload inválido.");

            var result = await _service.AtualizarAsync(id, request);
            if (result == null)
                return NotFound($"Curso com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var success = await _service.ExcluirAsync(id);
            if (!success)
                return NotFound($"Curso com id {id} não encontrado.");

            return NoContent();
        }
    }
}
