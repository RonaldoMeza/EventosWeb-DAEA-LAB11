# Eventos.Web Laboratorio 11 Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use executing-plans to implement this plan task-by-task.

**Goal:** Construir la aplicación MVC definida en `PLAN11.md`, preservando la intervención manual del usuario y el orden de las tres migraciones.

**Architecture:** ASP.NET Core MVC con controladores que dependen de repositorios. EF Core Code First usa un `DbContext`, Data Annotations y consultas LINQ asíncronas.

**Tech Stack:** .NET 10, C#, ASP.NET Core MVC, EF Core 10.0.13, SQL Server Express.

---

### Task 1: Modelo inicial y configuración

1. Agregar EF Core 10.0.13 y la cadena de conexión.
2. Crear las cuatro entidades iniciales, sin `Modalidad` ni `Telefono`.
3. Crear y registrar `EventosContext`, sin `OnModelCreating`.
4. Compilar y revisar el modelo.
5. Pedir al usuario ejecutar `InitialCreate` y actualizar la base.

### Task 2: Funcionalidad sobre el esquema inicial

1. Incorporar repositorios asíncronos y registrarlos con `AddScoped`.
2. Incorporar la carga inicial condicionada por `AnyAsync` después de existir el esquema.
3. Implementar controladores y vistas de eventos, asistentes e inscripciones.
4. Verificar compilación y flujos del esquema inicial.

### Task 3: Modalidad de evento

1. Agregar y validar `Evento.Modalidad`.
2. Pedir al usuario generar `AgregarModalidadEvento`.
3. Cambiar únicamente el `defaultValue` de la migración a `"Presencial"`.
4. Pedir al usuario aplicar la migración e incorporar la propiedad en la UI requerida.

### Task 4: Teléfono de asistente

1. Agregar `Asistente.Telefono` opcional, máximo 9 caracteres.
2. Pedir al usuario generar y aplicar `AgregarTelefonoAsistente`.
3. Incorporar la propiedad en Create e Index y verificar.

### Task 5: Verificación final

1. Compilar y probar las funcionalidades disponibles.
2. Revisar `PLAN11.md` punto por punto.
3. Confirmar tres migraciones y contrastarlas con `__EFMigrationsHistory` después de que el usuario las aplique.
4. Documentar evidencias manuales y comandos realmente ejecutados.
