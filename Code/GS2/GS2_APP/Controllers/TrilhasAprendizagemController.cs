using GS2_APP.Models;
using GS2_APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace GS2_APP.Controllers
{
    public class TrilhasAprendizagemController:Controller

    {
        private readonly TrilhaAprendizagemService _service;
        public TrilhasAprendizagemController(TrilhaAprendizagemService service) {
            _service = service;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            IEnumerable<TrilhaAprendizagemModel> model = await _service.BuscarTodos();
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(c => c.Titulo.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var data = await _service.Buscar(id.Value);
            return data == null ? RedirectToAction("Index") : View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrilhaAprendizagemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool response = await _service.Editar(model);
            return response ? RedirectToAction("Index") : View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            return (id is null or 0) ? null : View(await _service.Buscar(id.Value));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool delete = await _service.Deletar(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrilhaAprendizagemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool response = await _service.Criar(model);
            return response ? RedirectToAction("Index") : View(model);
        }
    }
}
