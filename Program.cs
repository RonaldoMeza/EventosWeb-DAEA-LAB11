using EventosWeb.Data;
using EventosWeb.Models;
using EventosWeb.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Eventos")));
builder.Services.AddScoped<EventoRepositorio>();
builder.Services.AddScoped<AsistenteRepositorio>();
builder.Services.AddScoped<InscripcionRepositorio>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventosContext>();

    if (!await context.Categorias.AnyAsync())
    {
        context.Categorias.AddRange(
            new Categoria { Nombre = "Tecnología" },
            new Categoria { Nombre = "Negocios" },
            new Categoria { Nombre = "Cultura" });
        await context.SaveChangesAsync();
    }

    if (!await context.Eventos.AnyAsync())
    {
        var categorias = await context.Categorias.OrderBy(c => c.Id).Take(3).ToListAsync();
        if (categorias.Count >= 3)
        {
            context.Eventos.AddRange(
                new Evento { Titulo = "Conferencia de desarrollo web", Fecha = DateTime.Today.AddDays(30).AddHours(9), Lugar = "Auditorio principal", Aforo = 200, Precio = 80, CategoriaId = categorias[0].Id },
                new Evento { Titulo = "Taller de inteligencia artificial", Fecha = DateTime.Today.AddDays(45).AddHours(15), Lugar = "Laboratorio 2", Aforo = 40, Precio = 120, CategoriaId = categorias[0].Id },
                new Evento { Titulo = "Encuentro de emprendimiento", Fecha = DateTime.Today.AddDays(60).AddHours(10), Lugar = "Centro de convenciones", Aforo = 300, Precio = 50, CategoriaId = categorias[1].Id },
                new Evento { Titulo = "Festival cultural", Fecha = DateTime.Today.AddDays(75).AddHours(11), Lugar = "Plaza central", Aforo = 1000, Precio = 0, CategoriaId = categorias[2].Id });
            await context.SaveChangesAsync();
        }
    }

    if (!await context.Asistentes.AnyAsync())
    {
        context.Asistentes.AddRange(
            new Asistente { Nombres = "Ana Torres", Dni = "70123456", Correo = "ana.torres@example.com" },
            new Asistente { Nombres = "Bruno Díaz", Dni = "71234567", Correo = "bruno.diaz@example.com" },
            new Asistente { Nombres = "Carla Rojas", Dni = "72345678", Correo = "carla.rojas@example.com" },
            new Asistente { Nombres = "Diego Flores", Dni = "73456789", Correo = "diego.flores@example.com" },
            new Asistente { Nombres = "Elena Vargas", Dni = "74567890", Correo = "elena.vargas@example.com" });
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
