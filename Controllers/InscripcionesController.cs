using EventosWeb.Models;
using EventosWeb.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventosWeb.Controllers;

public class InscripcionesController : Controller
{
    private readonly InscripcionRepositorio _inscripciones;
    public InscripcionesController(InscripcionRepositorio inscripciones) => _inscripciones = inscripciones;

    public async Task<IActionResult> Index() => View(await _inscripciones.ListarAsync());

    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View(new Inscripcion());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EventoId,AsistenteId")] Inscripcion inscripcion)
    {
        ModelState.Remove(nameof(Inscripcion.Evento));
        ModelState.Remove(nameof(Inscripcion.Asistente));
        var evento = await _inscripciones.ObtenerEventoActivoAsync(inscripcion.EventoId);
        if (evento is null)
            ModelState.AddModelError(nameof(Inscripcion.EventoId), "Seleccione un evento activo.");
        else
        {
            if (evento.Fecha < DateTime.Now)
                ModelState.AddModelError(nameof(Inscripcion.EventoId), "No se permiten inscripciones en eventos que ya pasaron.");
            if (await _inscripciones.ExisteConfirmadaAsync(evento.Id, inscripcion.AsistenteId))
                ModelState.AddModelError(nameof(Inscripcion.AsistenteId), "El asistente ya tiene una inscripción confirmada en este evento.");
            if (await _inscripciones.ContarConfirmadasAsync(evento.Id) >= evento.Aforo)
                ModelState.AddModelError(nameof(Inscripcion.EventoId), "El evento alcanzó su aforo máximo.");
        }
        if (inscripcion.AsistenteId <= 0 || !await _inscripciones.ExisteAsistenteActivoAsync(inscripcion.AsistenteId))
            ModelState.AddModelError(nameof(Inscripcion.AsistenteId), "Seleccione un asistente activo.");
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(inscripcion.EventoId, inscripcion.AsistenteId);
            return View(inscripcion);
        }
        await _inscripciones.CrearAsync(inscripcion);
        TempData["Mensaje"] = "Inscripción confirmada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        if (!await _inscripciones.CancelarAsync(id)) return NotFound();
        TempData["Mensaje"] = "Inscripción cancelada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int? eventoId = null, int? asistenteId = null)
    {
        ViewBag.Eventos = new SelectList(await _inscripciones.ListarEventosActivosAsync(), nameof(Evento.Id), nameof(Evento.Titulo), eventoId);
        ViewBag.Asistentes = new SelectList(await _inscripciones.ListarAsistentesActivosAsync(), nameof(Asistente.Id), nameof(Asistente.Nombres), asistenteId);
    }
}
