using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [ApiController]
    [Route("api/nivel")]
    public class NivelController : ControllerBase
    {
        private readonly NivelService _service;

        public NivelController(NivelService service)
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
                return NotFound($"Nível com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] NivelDto dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");

            var result = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(Obter), new { id = result.NivelId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] NivelDto dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");

            var result = await _service.AtualizarAsync(id, dto);
            if (result == null)
                return NotFound($"Nível com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var ok = await _service.ExcluirAsync(id);
            if (!ok)
                return NotFound($"Nível com id {id} não encontrado.");

            return NoContent();
        }
    }
}
