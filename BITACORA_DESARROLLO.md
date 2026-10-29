# Bitácora de desarrollo — Laboratorio 11

## Estado general

- Fase actual: finalizada y verificada.
- `InitialCreate` fue generada y aplicada por el usuario.
- La funcionalidad del esquema inicial está implementada y verificada.
- Pendiente del usuario: capturar las evidencias finales en SQL Server y la carpeta `Migrations`.
- El agente no ha ejecutado ningún comando de migración.

## Fase 1 — Modelo inicial

- EF Core SQL Server y Design 10.0.13 y conexión a `EventosDB`.
- Referencia `System.Security.Cryptography.Pkcs` 10.0.13 para corregir alertas de la versión transitiva 9.0.11.
- Entidades, Data Annotations, índices únicos, relaciones y navegaciones.
- `EventosContext` con cuatro `DbSet`, sin Fluent API, registrado con `AddDbContext`.
- `dotnet --version`: `10.0.401`.
- `dotnet ef --version`: `10.0.13`; ya estaba instalado globalmente.
- Compilación: 0 advertencias y 0 errores.

## Punto de control — InitialCreate

- El usuario ejecutó `dotnet ef migrations add InitialCreate` y `dotnet ef database update`.
- Migración: `20261028225444_InitialCreate`.
- La salida confirma `EventosDB`, cuatro tablas, índices, claves foráneas y `__EFMigrationsHistory`.
- Historial confirmado: `20261028225444_InitialCreate`, producto `10.0.13`.

## Fase 2 — Funcionalidad inicial

- Tres repositorios asíncronos registrados con `AddScoped`.
- LINQ, `AsNoTracking` en lecturas y `SaveChangesAsync` en escrituras.
- Carga inicial con `AnyAsync`: 3 categorías, 4 eventos y 5 asistentes.
- Controladores y vistas tipadas de eventos, asistentes e inscripciones.
- Filtro, orden, confirmados, cupos, CRUD de eventos y eliminación lógica.
- Alta de asistentes con validación de DNI/correo duplicados.
- Validaciones, alta y cancelación de inscripciones.
- Menú principal requerido.

### Verificaciones reales

- `dotnet build`: 0 advertencias y 0 errores.
- Primer arranque: insertó realmente los 3/4/5 registros iniciales.
- HTTP 200 en las páginas Index y Create de los tres módulos.
- La aplicación de prueba se cerró normalmente.

## Fase 3 — Modalidad del evento

- Preparado `Evento.Modalidad`: obligatoria, máximo 20, valores `Presencial` o `Virtual`.
- El usuario generó `20261028230751_AgregarModalidadEvento`.
- Se revisó el archivo y se cambió únicamente su `defaultValue` de cadena vacía a `"Presencial"`.
- Modalidad se incorporó en Create, Edit, Index y Details de eventos.
- La migración quedó preparada y posteriormente fue aplicada por el usuario.

### Aplicación confirmada

- El usuario ejecutó `dotnet ef database update`.
- La salida confirma `ALTER TABLE [Eventos] ADD [Modalidad] nvarchar(20) NOT NULL DEFAULT N'Presencial'`.
- `__EFMigrationsHistory` registró `20261028230751_AgregarModalidadEvento`, producto `10.0.13`.

## Fase 4 — Teléfono del asistente

- Preparado `Asistente.Telefono`, opcional y con máximo 9 caracteres.
- Incorporado en Create e Index de asistentes.
- El usuario generó `20261028232245_AgregarTelefonoAsistente`.
- Se revisó: agrega únicamente `Telefono` como `nvarchar(9)`, máximo 9 y nullable.
- La carpeta contiene exactamente tres migraciones y el snapshot.
- El usuario aplicó la migración; la salida confirma `ALTER TABLE [Asistentes] ADD [Telefono] nvarchar(9) NULL`.
- `__EFMigrationsHistory` registró `20261028232245_AgregarTelefonoAsistente`, producto `10.0.13`.
- Compilación de la preparación: 0 advertencias y 0 errores.

## Lista de verificación de PLAN11.md

- [x] 1-6. Code First, entidades, atributos, contexto y configuración MVC/EF.
- [x] 7. Datos iniciales condicionados.
- [x] 8. Repositorios y `AddScoped`.
- [x] 9-10. Eventos, patrón PRG, TempData y vistas tipadas.
- [x] 11. Asistentes y duplicados.
- [x] 12. Inscripciones y reglas.
- [x] 13. Async, `AsNoTracking` y LINQ.
- [x] 14. Menú principal.
- [x] 15. `InitialCreate` aplicada por el usuario.
- [x] 16. `AgregarModalidadEvento` y UI.
- [x] 17. `AgregarTelefonoAsistente` y UI.
- [x] 18. Exactamente tres migraciones y snapshot.
- [ ] 19. Capturas finales del historial y carpeta (intervención del usuario).
- [x] 20-22. VS Code, `dotnet-ef` y certificado.

## Verificación final

- `dotnet test`: restauración y compilación correctas; el proyecto no contiene un proyecto de pruebas automatizadas.
- `dotnet ef migrations list`: mostró las tres migraciones aplicadas y ninguna pendiente.
- Se confirmó que los controladores no inyectan `EventosContext` directamente.
- No se encontró `EnsureCreated`, `OnModelCreating` ni `FromSqlRaw`.
- Las seis páginas principales respondieron HTTP 200 con el esquema definitivo.
- Eventos Index/Create muestran Modalidad; Asistentes Index/Create muestran Teléfono.
- La aplicación de verificación se cerró normalmente.

## Evidencias manuales pendientes

Ejecutar en SQL Server sobre `EventosDB`:

```sql
SELECT MigrationId, ProductVersion
FROM __EFMigrationsHistory
ORDER BY MigrationId;
```

Capturar las tres filas y la carpeta `Migrations` expandida con los tres pares de archivos y `EventosContextModelSnapshot.cs`.
