using EventosWeb.Models;
using EventosWeb.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace EventosWeb.Controllers;

public class AsistentesController : Controller
{
    private readonly AsistenteRepositorio _asistentes;
    public AsistentesController(AsistenteRepositorio asistentes) => _asistentes = asistentes;

    public async Task<IActionResult> Index() => View(await _asistentes.ListarActivosAsync());

    public IActionResult Create() => View(new Asistente());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombres,Dni,Correo,Telefono")] Asistente asistente)
    {
        if (!string.IsNullOrWhiteSpace(asistente.Dni) && await _asistentes.ExisteDniAsync(asistente.Dni))
            ModelState.AddModelError(nameof(Asistente.Dni), "Ya existe un asistente con este DNI.");
        if (!string.IsNullOrWhiteSpace(asistente.Correo) && await _asistentes.ExisteCorreoAsync(asistente.Correo))
            ModelState.AddModelError(nameof(Asistente.Correo), "Ya existe un asistente con este correo.");
        if (!ModelState.IsValid) return View(asistente);
        await _asistentes.CrearAsync(asistente);
        TempData["Mensaje"] = $"Asistente \"{asistente.Nombres}\" registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
