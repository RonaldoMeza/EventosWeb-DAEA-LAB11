using EventosWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosWeb.Data;

public class EventosContext : DbContext
{
    public EventosContext(DbContextOptions<EventosContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Asistente> Asistentes => Set<Asistente>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
}
