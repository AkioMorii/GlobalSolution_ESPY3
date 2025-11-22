using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }
        /// <summary>
        /// Metodo: Listar
        /// </summary>
        /// <returns>Lista de Usuarios</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.ListarAsync();
            return Ok(lista);
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var usuario = await _service.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var novoUsuario = await _service.CriarAsync(dto);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = novoUsuario.UsuarioId },
                novoUsuario
            );
        }
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var atualizado = await _service.AtualizarAsync(id, dto);

            if (atualizado == null)
                return NotFound();

            return Ok(atualizado);
        }
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var sucesso = await _service.ExcluirAsync(id);
            return sucesso ? NoContent() : NotFound();
        }
        

        [Authorize(Roles = "Administrador")]
        [HttpPost("{id:int}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] UsuarioAlterarSenhaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sucesso = await _service.AlterarSenhaAsync(id, dto);

            if (!sucesso)
                return BadRequest("Senha atual incorreta ou usuário não encontrado.");

            return Ok(new { mensagem = "Senha alterada com sucesso." });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("{id:int}/desativar")]
        public async Task<IActionResult> Desativar(int id)
        {
            var sucesso = await _service.DesativarAsync(id);
            return sucesso ? Ok() : NotFound();
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("{id:int}/ativar")]
        public async Task<IActionResult> Ativar(int id)
        {
            var sucesso = await _service.AtivarAsync(id);
            return sucesso ? Ok() : NotFound();
        }
    }
}
