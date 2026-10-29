using EventosWeb.Data;
using EventosWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosWeb.Repositorios;

public class InscripcionRepositorio
{
    private readonly EventosContext _context;
    public InscripcionRepositorio(EventosContext context) => _context = context;

    public async Task<List<Inscripcion>> ListarAsync() => await _context.Inscripciones.AsNoTracking()
        .Include(i => i.Evento).Include(i => i.Asistente).OrderByDescending(i => i.FechaInscripcion).ToListAsync();
    public async Task<List<Evento>> ListarEventosActivosAsync() => await _context.Eventos.AsNoTracking()
        .Where(e => e.Activo).OrderBy(e => e.Fecha).ToListAsync();
    public async Task<List<Asistente>> ListarAsistentesActivosAsync() => await _context.Asistentes.AsNoTracking()
        .Where(a => a.Activo).OrderBy(a => a.Nombres).ToListAsync();
    public async Task<Evento?> ObtenerEventoActivoAsync(int id) => await _context.Eventos.AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    public async Task<bool> ExisteAsistenteActivoAsync(int id) => await _context.Asistentes.AnyAsync(a => a.Id == id && a.Activo);
    public async Task<bool> ExisteConfirmadaAsync(int eventoId, int asistenteId) => await _context.Inscripciones
        .AnyAsync(i => i.EventoId == eventoId && i.AsistenteId == asistenteId && i.Estado == "Confirmada");
    public async Task<int> ContarConfirmadasAsync(int eventoId) => await _context.Inscripciones
        .CountAsync(i => i.EventoId == eventoId && i.Estado == "Confirmada");

    public async Task CrearAsync(Inscripcion inscripcion)
    {
        inscripcion.FechaInscripcion = DateTime.Now;
        inscripcion.Estado = "Confirmada";
        _context.Inscripciones.Add(inscripcion);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var inscripcion = await _context.Inscripciones.FindAsync(id);
        if (inscripcion is null || inscripcion.Estado != "Confirmada") return false;
        inscripcion.Estado = "Cancelada";
        await _context.SaveChangesAsync();
        return true;
    }
}
