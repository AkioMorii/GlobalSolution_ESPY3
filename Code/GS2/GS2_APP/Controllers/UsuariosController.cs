
using GS2_APP.Models;
using GS2_APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GS2_APP.Controllers
{
    [Authorize (Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usurioService;
        public UsuariosController(UsuarioService usurioService)
        {
            _usurioService= usurioService;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            IEnumerable<UsuarioModel> usuarios = await _usurioService.Buscar();
            return View(usuarios);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var usuario = await BuscarValido(id);
            return usuario == null ? RedirectToAction("Index") : View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(UsuarioModel model)
        {
            ModelState.Remove("Senha");
            ModelState.Remove("ConfirmarSenha");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool response = await _usurioService.Editar(model);
            return response ? RedirectToAction("Index") : View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var usuario = await BuscarValido(id);
            return usuario == null ? RedirectToAction("Index") : View(usuario);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(UsuarioModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool response=await _usurioService.Criar(model);
            return response ? RedirectToAction("Index") : View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            bool delete= await _usurioService.Deletar(id);
            return RedirectToAction("Index");
        }
        private async Task<UsuarioModel?> BuscarValido(int? id)
        {
            return (id is null or 0) ? null : await _usurioService.Buscar(id.Value);
        }
    }
}
