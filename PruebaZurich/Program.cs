using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Implementations;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Services.Implementations;
using PruebaZurich.Services.Interfaces;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración centralizada
var configuration = builder.Configuration;


builder.Services.AddControllers();

// Configuración de DbContext
builder.Services.AddDbContext<ZurichDBContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Configuración de AutoMapper (versión optimizada)
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Configuración de Repositorios
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPolizaRepository, PolizaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Configuración de Servicios
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPolizaService, PolizaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuración de JWT con validación
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ??
    throw new InvalidOperationException("JWT Secret no configurado en appsettings");


builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configuración de CORS (movido antes de builder.Build())
var corsConfig = configuration.GetSection("CorsPolicy");
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        // 1. Orígenes permitidos (configuración mejorada)
        var allowedOrigins = corsConfig.GetValue<string[]>("AllowedOrigins") ?? Array.Empty<string>();

        // 2. Agrega automáticamente localhost en desarrollo si no hay orígenes configurados
        if (builder.Environment.IsDevelopment() && allowedOrigins.Length == 0)
        {
            allowedOrigins = new[] { "http://localhost", "http://localhost:4200", "https://localhost:4200" };
        }

        // 3. Configuración de política
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetPreflightMaxAge(TimeSpan.FromSeconds(
                      corsConfig.GetValue<int>("PreflightMaxAge", 600)));

            if (corsConfig.GetValue<bool>("AllowCredentials", false))
            {
                policy.AllowCredentials();
            }
        }
        else if (builder.Environment.IsDevelopment())
        {
            // Solo en desarrollo permite cualquier origen (útil para pruebas)
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zurich Seguros API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

if (!builder.Environment.IsDevelopment())
{
    // Para producción (Azure) usa puerto 80 en todas las interfaces
    builder.WebHost.UseUrls("http://*:80");
}

var app = builder.Build();

// Configuración del Pipeline HTTP
// app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("DefaultPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicialización de la Base de Datos
await InitializeDatabase(app);

app.Run();

// Métodos auxiliares
async Task InitializeDatabase(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ZurichDBContext>();
        var logger = webApp.Logger;

        // Aplicar migraciones
        await context.Database.MigrateAsync();
        logger.LogInformation("Migraciones aplicadas correctamente");

        // Verificar y crear usuario admin con manejo de concurrencia
        const string adminEmail = "admin@zurich.com";
        
        if (!await context.Usuarios.AnyAsync(u => u.Email == adminEmail))
        {
            try 
            {
                var (hash, salt) = CreatePasswordHash("Admin123*");

                context.Usuarios.Add(new Usuario
                {
                    NombreUsuario = "admin",
                    Email = adminEmail,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Rol = "Administrador",
                    FechaCreacion = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
                logger.LogInformation("Usuario admin creado exitosamente");
            }
            catch (DbUpdateException ex) when (IsDuplicateKeyError(ex))
            {
                logger.LogWarning("El usuario admin ya existía");
            }
        }
        else
        {
            logger.LogInformation("Usuario admin ya existe en la base de datos");
        }
    }
    catch (Exception ex)
    {
        webApp.Logger.LogError(ex, "Error durante la inicialización de la base de datos");
        throw; // Relanzar para que no pase desapercibido en producción
    }
}

// Método auxiliar para detectar errores de clave duplicada
bool IsDuplicateKeyError(DbUpdateException ex)
{
    return ex.InnerException is SqlException sqlEx && 
           (sqlEx.Number == 2601 || sqlEx.Number == 2627);
}

(byte[] hash, byte[] salt) CreatePasswordHash(string password)
{
    using var hmac = new HMACSHA512();
    return (
        hash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
        salt: hmac.Key
    );
}