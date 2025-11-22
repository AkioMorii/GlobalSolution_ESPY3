using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [Authorize(Roles = "Administrador,Instrutor")]
    [Route("api/tipoconteudo")]
    [ApiController]
    public class TipoConteudoController : ControllerBase
    {
        private readonly TiposConteudoService _service;

        public TipoConteudoController(TiposConteudoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(TipoConteudoDto dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest("O campo 'descricao' é obrigatório.");

            var result = await _service.CriarAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var result = await _service.ObterPorIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string? descricao)
        {
            var result = await _service.BuscarAsync(descricao);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, TipoConteudoDto dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");

            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest("O campo 'descricao' é obrigatório.");

            var result = await _service.AtualizarAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            await _service.ExcluirAsync(id);
            return NoContent();
        }
    }
}
