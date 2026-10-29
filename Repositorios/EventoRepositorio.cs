using EventosWeb.Data;
using EventosWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosWeb.Repositorios;

public class EventoRepositorio
{
    private readonly EventosContext _context;
    public EventoRepositorio(EventosContext context) => _context = context;

    public async Task<List<Evento>> ListarAsync(int? categoriaId)
    {
        var consulta = _context.Eventos.AsNoTracking().Include(e => e.Categoria)
            .Include(e => e.Inscripciones).Where(e => e.Activo);
        if (categoriaId.HasValue) consulta = consulta.Where(e => e.CategoriaId == categoriaId.Value);
        return await consulta.OrderBy(e => e.Fecha).ToListAsync();
    }

    public async Task<Evento?> ObtenerAsync(int id) => await _context.Eventos.AsNoTracking()
        .Include(e => e.Categoria).Include(e => e.Inscripciones)
        .FirstOrDefaultAsync(e => e.Id == id && e.Activo);

    public async Task<List<Categoria>> ListarCategoriasActivasAsync() => await _context.Categorias.AsNoTracking()
        .Where(c => c.Activo).OrderBy(c => c.Nombre).ToListAsync();
    public async Task<bool> ExisteCategoriaActivaAsync(int id) => await _context.Categorias.AnyAsync(c => c.Id == id && c.Activo);

    public async Task CrearAsync(Evento evento)
    {
        evento.Activo = true;
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(Evento evento)
    {
        var actual = await _context.Eventos.FindAsync(evento.Id);
        if (actual is null || !actual.Activo) return false;
        actual.Titulo = evento.Titulo; actual.Fecha = evento.Fecha; actual.Lugar = evento.Lugar;
        actual.Aforo = evento.Aforo; actual.Precio = evento.Precio; actual.Modalidad = evento.Modalidad;
        actual.CategoriaId = evento.CategoriaId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento is null || !evento.Activo) return false;
        evento.Activo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
