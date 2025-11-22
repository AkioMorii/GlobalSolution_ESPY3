using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using GS2_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    //[Authorize(Roles = "Administrador,Instrutor")]
    [AllowAnonymous]
    [ApiController]
    [Route("api/tipousuario")]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly TipoUsuarioService _service;
        private readonly IMapper _mapper;

        public TipoUsuarioController(TipoUsuarioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarAsync();
            return Ok(_mapper.Map<List<TipoUsuarioDto>>(result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var entity = await _service.ObterPorIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<TipoUsuarioDto>(entity));
        }

        [HttpGet("busca")]
        public async Task<IActionResult> BuscarPorDescricao([FromQuery] string descricao)
        {
            var result = await _service.BuscarPorDescricaoAsync(descricao);
            return Ok(_mapper.Map<List<TipoUsuarioDto>>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] TipoUsuarioDto dto)
        {
            var entity = _mapper.Map<TipoUsuario>(dto);
            var result = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(ObterPorId), new { id = result.TipoUsuarioId, version = "1.0" }, _mapper.Map<TipoUsuarioDto>(result));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TipoUsuarioDto dto)
        {
            if (id != dto.TipoUsuarioId) return BadRequest();
            var entity = _mapper.Map<TipoUsuario>(dto);
            await _service.AtualizarAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _service.DeletarAsync(id);
            return NoContent();
        }
    }
}
