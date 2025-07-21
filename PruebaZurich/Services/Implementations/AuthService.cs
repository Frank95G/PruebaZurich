using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Models.DTOs.Auth;
using PruebaZurich.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using PruebaZurich.Exceptions;

namespace PruebaZurich.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDto)
        {
            var usuario = await _unitOfWork.Usuarios.GetByUser(loginDto.Username);
            if (usuario == null)
                throw new UnauthorizedException("Credenciales inválidas");

            if (!VerifyPasswordHash(loginDto.Password, usuario.PasswordHash, usuario.PasswordSalt))
                throw new UnauthorizedException("Credenciales inválidas");

            var cliente = await _unitOfWork.Clientes.GetByUsuarioId(usuario.UsuarioId);

            return await GenerateAuthResponse(usuario, cliente != null ? cliente.ClienteId : 0);
        }

        public async Task<AuthResponseDTO> Registrar(RegisterDTO registroDto)
        {
            if (await _unitOfWork.Usuarios.ExistsByEmail(registroDto.Email))
                throw new BusinessException("El email ya está registrado");

            CreatePasswordHash(registroDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuario = _mapper.Map<Usuario>(registroDto);
            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;
            usuario.Rol = "Cliente"; // Rol por defecto
            var clienteId = 0;

            // Primero guardamos el usuario para obtener su ID generado
            await _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.Complete(); // Guarda cambios para obtener el ID

            // Verificamos si existe el cliente con ese email
            if (await _unitOfWork.Clientes.ExistsByEmail(registroDto.Email))
            {
                var cliente = await _unitOfWork.Clientes.GetByEmail(registroDto.Email);
                cliente.UsuarioId = usuario.UsuarioId; // Asignamos el ID del usuario recién creado
                clienteId = cliente.ClienteId;

                _unitOfWork.Clientes.Update(cliente);
                await _unitOfWork.Complete(); // Guardamos la relación
            }

            await _unitOfWork.Complete();

            return await GenerateAuthResponse(usuario, clienteId);
        }

        public async Task<AuthResponseDTO> RegistrarAdministrador(RegisterDTO registroDto)
        {
            if (await _unitOfWork.Usuarios.ExistsByEmail(registroDto.Email))
                throw new BusinessException("El email ya está registrado");

            CreatePasswordHash(registroDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var usuario = _mapper.Map<Usuario>(registroDto);
            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;
            usuario.Rol = "Administrador"; // Rol específico para administradores

            await _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.Complete();

            return await GenerateAuthResponse(usuario, 0);
        }

        public async Task<AuthResponseDTO> GenerateAuthResponse(Usuario usuario, int? clienteId)
        {
            var response = new AuthResponseDTO
            {
                Token = GenerateJwtToken(usuario),
                Usuario = _mapper.Map<UsuarioDTO>(usuario)
            };
            response.Usuario.ClienteId = clienteId;

            return response;

        }

        public async Task<bool> ExisteUsuario(string email)
        {
            return await _unitOfWork.Usuarios.ExistsByEmail(email);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
                Audience = _configuration.GetSection("JwtSettings:Audience").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
