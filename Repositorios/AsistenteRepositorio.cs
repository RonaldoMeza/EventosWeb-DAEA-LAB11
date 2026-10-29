using EventosWeb.Data;
using EventosWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosWeb.Repositorios;

public class AsistenteRepositorio
{
    private readonly EventosContext _context;
    public AsistenteRepositorio(EventosContext context) => _context = context;

    public async Task<List<Asistente>> ListarActivosAsync() => await _context.Asistentes.AsNoTracking()
        .Where(a => a.Activo).OrderBy(a => a.Nombres).ToListAsync();
    public async Task<bool> ExisteDniAsync(string dni) => await _context.Asistentes.AnyAsync(a => a.Dni == dni);
    public async Task<bool> ExisteCorreoAsync(string correo) => await _context.Asistentes.AnyAsync(a => a.Correo == correo);

    public async Task CrearAsync(Asistente asistente)
    {
        asistente.Activo = true;
        _context.Asistentes.Add(asistente);
        await _context.SaveChangesAsync();
    }
}
