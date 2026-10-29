using EventosWeb.Models;
using EventosWeb.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventosWeb.Controllers;

public class EventosController : Controller
{
    private readonly EventoRepositorio _eventos;
    public EventosController(EventoRepositorio eventos) => _eventos = eventos;

    public async Task<IActionResult> Index(int? categoriaId)
    {
        await CargarCategoriasAsync(categoriaId, true);
        ViewBag.CategoriaId = categoriaId;
        return View(await _eventos.ListarAsync(categoriaId));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var evento = await _eventos.ObtenerAsync(id.Value);
        return evento is null ? NotFound() : View(evento);
    }

    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();
        return View(new Evento { Fecha = DateTime.Today.AddDays(1).AddHours(9), Aforo = 1 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,Fecha,Lugar,Aforo,Precio,Modalidad,CategoriaId")] Evento evento)
    {
        ModelState.Remove(nameof(Evento.Categoria));
        await ValidarCategoriaAsync(evento.CategoriaId);
        if (!ModelState.IsValid)
        {
            await CargarCategoriasAsync(evento.CategoriaId);
            return View(evento);
        }
        await _eventos.CrearAsync(evento);
        TempData["Mensaje"] = $"Evento \"{evento.Titulo}\" registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var evento = await _eventos.ObtenerAsync(id.Value);
        if (evento is null) return NotFound();
        await CargarCategoriasAsync(evento.CategoriaId);
        return View(evento);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Fecha,Lugar,Aforo,Precio,Modalidad,CategoriaId")] Evento evento)
    {
        if (id != evento.Id) return NotFound();
        ModelState.Remove(nameof(Evento.Categoria));
        await ValidarCategoriaAsync(evento.CategoriaId);
        if (!ModelState.IsValid)
        {
            await CargarCategoriasAsync(evento.CategoriaId);
            return View(evento);
        }
        if (!await _eventos.ActualizarAsync(evento)) return NotFound();
        TempData["Mensaje"] = $"Evento \"{evento.Titulo}\" actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var evento = await _eventos.ObtenerAsync(id.Value);
        return evento is null ? NotFound() : View(evento);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var evento = await _eventos.ObtenerAsync(id);
        if (evento is null) return NotFound();
        await _eventos.DesactivarAsync(id);
        TempData["Mensaje"] = $"Evento \"{evento.Titulo}\" desactivado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarCategoriaAsync(int id)
    {
        if (id <= 0 || !await _eventos.ExisteCategoriaActivaAsync(id))
            ModelState.AddModelError(nameof(Evento.CategoriaId), "Seleccione una categoría activa.");
    }

    private async Task CargarCategoriasAsync(int? seleccionado = null, bool incluirTodos = false)
    {
        var categorias = await _eventos.ListarCategoriasActivasAsync();
        ViewBag.Categorias = new SelectList(categorias, nameof(Categoria.Id), nameof(Categoria.Nombre), seleccionado);
        ViewBag.IncluirTodos = incluirTodos;
    }
}
