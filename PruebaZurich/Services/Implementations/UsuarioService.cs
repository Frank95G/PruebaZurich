using AutoMapper;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Models.DTOs.Auth;
using PruebaZurich.Models.DTOs.Shared;
using PruebaZurich.Services.Interfaces;
using PruebaZurich.Exceptions;

namespace PruebaZurich.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IAuthService _authService;

        public UsuarioService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UsuarioService> logger,
            IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
        }

        public async Task<AuthResponseDTO> RegistrarAdministrador(RegisterDTO registro)
        {
            if (await _unitOfWork.Usuarios.ExistsByEmail(registro.Email))
                throw new BusinessException("El email ya está registrado");

            var usuario = _mapper.Map<Usuario>(registro);
            usuario.Rol = "Administrador";

            await _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.Complete();

            return await _authService.GenerateAuthResponse(usuario, 0);
        }

        public async Task<AuthResponseDTO> RegistrarCliente(RegisterDTO registro)
        {
            if (await _unitOfWork.Usuarios.ExistsByEmail(registro.Email))
                throw new BusinessException("El email ya está registrado");

            var usuario = _mapper.Map<Usuario>(registro);
            usuario.Rol = "Cliente";

            await _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.Complete();

            return await _authService.GenerateAuthResponse(usuario, 0);
        }

        public async Task<PagedResultDTO<UsuarioDTO>> ListarUsuarios(int pageNumber = 1, int pageSize = 10)
        {
            var usuarios = await _unitOfWork.Usuarios.GetAll();
            var total = await _unitOfWork.Usuarios.CountAsync();

            return new PagedResultDTO<UsuarioDTO>
            {
                Items = (List<UsuarioDTO>)_mapper.Map<IEnumerable<UsuarioDTO>>(usuarios),
                TotalItems = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UsuarioDTO> ObtenerUsuario(int id)
        {
            var usuario = await _unitOfWork.Usuarios.GetById(id);
            if (usuario == null)
                throw new NotFoundException("Usuario no encontrado");

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task AsignarRol(int usuarioId, string rol)
        {
            if (rol != "Administrador" && rol != "Cliente")
                throw new BusinessException("Rol no válido");

            var usuario = await _unitOfWork.Usuarios.GetById(usuarioId);
            if (usuario == null)
                throw new NotFoundException("Usuario no encontrado");

            usuario.Rol = rol;
            _unitOfWork.Usuarios.Update(usuario);
            await _unitOfWork.Complete();
        }
    }
}
