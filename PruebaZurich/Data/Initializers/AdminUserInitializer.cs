using Microsoft.EntityFrameworkCore;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PruebaZurich.Data.Initializers
{
    public static class AdminUserInitializer
    {
        public static async Task InitializeAsync(ZurichDBContext context, IUsuarioRepository usuarioRepo)
        {
            // Verificar si ya existe el admin
            var adminExists = await usuarioRepo.GetByEmail("admin@zurich.com");
            if (adminExists != null) return;

            // Crear hash de contraseña
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("AdminZurich2024!"));
            var passwordSalt = hmac.Key;

            // Crear usuario admin
            var adminUser = new Usuario
            {
                NombreUsuario = "admin",
                Email = "admin@zurich.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Rol = "Administrador",
                FechaCreacion = DateTime.UtcNow
            };

            await usuarioRepo.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}