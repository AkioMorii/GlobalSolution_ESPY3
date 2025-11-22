using GS2_APP.Models;
using GS2_APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace GS2_APP.Controllers
{
    [Authorize(Roles = "Instrutor,Administrador")]
    public class CursosController : Controller
    {
        private readonly CursoService _cursoService;
        private readonly NivelService _nivelService;
        private readonly TrilhaAprendizagemService _trilhaService;
        private readonly PalavraChaveService _palavraService;
        private readonly TipoConteudoService _tipoConteudoService;

        public CursosController(
            CursoService cursoService,
            NivelService nivelService,
            TrilhaAprendizagemService trilhaService,
            PalavraChaveService palavraService,
            TipoConteudoService tipoConteudoService)
        {
            _cursoService = cursoService;
            _nivelService = nivelService;
            _trilhaService = trilhaService;
            _palavraService = palavraService;
            _tipoConteudoService = tipoConteudoService;

        }

        // ------------------- LISTAGEM -------------------
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            IEnumerable<CursoModel> cursos = await _cursoService.BuscarTodos();
            if (!string.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(c => c.Titulo.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            return View(cursos);
        }

        // ------------------- DETAILS -------------------
        public async Task<IActionResult> Details(int? id)
        {
            var curso = await BuscarValido(id);
            return curso == null ? RedirectToAction("Index") : View(curso);
        }

        // ------------------- CREATE -------------------
        public async Task<IActionResult> Create()
        {
            var model = await PopularDados(new CursoModel());
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CursoModel model, IEnumerable<IFormFile> arquivos, string ConteudosJson)
        {
            List<ConteudoModel> conteudosInfo = new List<ConteudoModel>();

            if (!string.IsNullOrWhiteSpace(ConteudosJson))
            {
                conteudosInfo = JsonSerializer.Deserialize<List<ConteudoModel>>(ConteudosJson)
                                 ?? new List<ConteudoModel>();
            }

            model.Conteudos = conteudosInfo;

            foreach (var c in conteudosInfo)
            {
                model.Conteudos.Add(new ConteudoModel
                {
                    NomeArquivo = c.NomeArquivo,
                    Url = "",
                    TipoConteudoId = c.TipoConteudoId,
                    Ordem = c.Ordem
                });
            }
            if (arquivos != null && arquivos.Any())
            {
                foreach (var file in arquivos)
                {
                    // Associa pelo NomeArquivo (que o JS mantém)
                    var conteudo = model.Conteudos.FirstOrDefault(x => x.NomeArquivo == file.FileName);
                    if (conteudo != null)
                    {
                        conteudo.Arquivo = file; // <-- Aqui preenche o IFormFile
                    }
                    else
                    {
                        // Caso o arquivo seja novo e não exista no JSON
                        model.Conteudos.Add(new ConteudoModel
                        {
                            NomeArquivo = file.FileName,
                            Arquivo = file,
                            Url = "",
                            Ordem = model.Conteudos.Count + 1,
                            TipoConteudoId = 0 // ou outro valor default
                        });
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                model = await PopularDados(model);
                return View("CreateEdit", model);
            }

            bool resultado = await _cursoService.Criar(model, arquivos);
            return resultado ? RedirectToAction("Index") : View("CreateEdit", await PopularDados(model));
        }

        // ------------------- EDIT -------------------
        public async Task<IActionResult> Edit(int? id)
        {
            var curso = await BuscarValido(id);
            return curso == null ? RedirectToAction("Index") : View("CreateEdit", await PopularDados(curso));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CursoModel model, IEnumerable<IFormFile> arquivos, string ConteudosJson)
        {
            // 1) Desserializa os metadados do JSON (novos conteúdos adicionados no front)
            List<ConteudoModel> conteudosInfo = new List<ConteudoModel>();
            if (!string.IsNullOrWhiteSpace(ConteudosJson))
            {
                conteudosInfo = JsonSerializer.Deserialize<List<ConteudoModel>>(ConteudosJson)
                                 ?? new List<ConteudoModel>();
            }

            // 2) Inicializa a lista de conteúdos da model, mantendo os existentes
            var updatedConteudos = new List<ConteudoModel>();

            // Mantém conteúdos já existentes que têm Url preenchida
            if (model.Conteudos != null)
            {
                updatedConteudos.AddRange(model.Conteudos.Where(c => !string.IsNullOrWhiteSpace(c.Url)));
            }

            // Adiciona os novos conteúdos do JSON (sem Url, serão arquivos novos)
            foreach (var c in conteudosInfo)
            {
                updatedConteudos.Add(new ConteudoModel
                {
                    NomeArquivo = c.NomeArquivo,
                    Url = "",
                    TipoConteudoId = c.TipoConteudoId,
                    Ordem = c.Ordem
                });
            }

            // 3) Associa os arquivos enviados aos conteúdos correspondentes
            if (arquivos != null && arquivos.Any())
            {
                foreach (var file in arquivos)
                {
                    var conteudo = updatedConteudos.FirstOrDefault(x => x.NomeArquivo == file.FileName);
                    if (conteudo != null)
                    {
                        conteudo.Arquivo = file;
                    }
                    else
                    {
                        // Arquivo totalmente novo
                        updatedConteudos.Add(new ConteudoModel
                        {
                            NomeArquivo = file.FileName,
                            Arquivo = file,
                            Url = "",
                            Ordem = updatedConteudos.Count + 1,
                            TipoConteudoId = 0 // ou valor default
                        });
                    }
                }
            }

            model.Conteudos = updatedConteudos;

            // 4) Validação do ModelState
            if (!ModelState.IsValid)
            {
                return View("CreateEdit", await PopularDados(model));
            }

            // 5) Envia para o service
            bool resultado = await _cursoService.Editar(model, arquivos);
            return resultado ? RedirectToAction("Index") : View("CreateEdit", await PopularDados(model));
        }

        // ------------------- DELETE -------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _cursoService.Deletar(id);
            return RedirectToAction("Index");
        }

        // ------------------- MÉTODOS AUXILIARES -------------------

        // Busca curso válido ou retorna null
        private async Task<CursoModel?> BuscarValido(int? id)
        {
            return (id is null or 0) ? null : await _cursoService.Buscar(id.Value);
        }

        // Popular listas de seleção (Níveis, Trilhas, Palavras)
        private async Task<CursoModel> PopularDados(CursoModel model)
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);
            string userName = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            model.ProprietarioId = userId;
            model.ProprietarioNome = userName;
            model.ListaTiposConteudos = (await _tipoConteudoService.BuscarTodos()).ToList();
            model.ListaNiveis = (await _nivelService.BuscarTodos()).ToList();
            //model.ListaTrilhas = (await _trilhaService.BuscarTodos()).ToList();
            model.ListaPalavrasChave = (await _palavraService.BuscarTodos()).ToList();
           
            return model;
        }
    }
}
