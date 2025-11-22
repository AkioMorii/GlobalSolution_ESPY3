using GS2_API.DTOs.v1;
using GS2_API.Services.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS2_API.Controllers.v1
{
    [Authorize(Roles = "Administrador,Instrutor")]
    [Route("api/conteudo")]
    [ApiController]
    public class ConteudoController : ControllerBase
    {
        private readonly ConteudoService _service;
        private readonly IConfiguration _config;

        public ConteudoController(ConteudoService service,IConfiguration config)
        {
            _service = service;
            _config = config;
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
                return NotFound($"Conteúdo com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ConteudoDto request)
        {
            if (request == null)
                return BadRequest("Payload inválido.");

            var result = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(Obter), new { id = result.ConteudoId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ConteudoDto request)
        {
            if (request == null)
                return BadRequest("Payload inválido.");

            var result = await _service.AtualizarAsync(id, request);
            if (result == null)
                return NotFound($"Conteúdo com id {id} não encontrado.");

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var success = await _service.ExcluirAsync(id);
            if (!success)
                return NotFound($"Conteúdo com id {id} não encontrado.");

            return NoContent();
        }
        [HttpPost("upload")]
        public async Task<ActionResult<List<ConteudoDto>>> Upload([FromForm] List<IFormFile> arquivos)
        {
            if (arquivos == null || !arquivos.Any())
                return BadRequest("Nenhum arquivo enviado.");

            var uploadPath = _config.GetValue<string>("FileUpload:BasePath");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var result = new List<ConteudoDto>();

            foreach (var file in arquivos)
            {
                if (file.Length > 0)
                {
                    var nomeArquivo = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadPath, nomeArquivo);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    result.Add(new ConteudoDto
                    {
                        NomeArquivo = file.FileName,
                        Url = $"/uploads/{nomeArquivo}"
                    });
                }
            }

            return Ok(result);
        }
    }
}
