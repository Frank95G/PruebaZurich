using AutoMapper;
using PruebaZurich.Data.Entities;
using PruebaZurich.Models.DTOs.Auth;
using PruebaZurich.Models.DTOs.Clientes;
using PruebaZurich.Models.DTOs.Polizas;

namespace PruebaZurich.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
        }
    }
    public class PolizaProfile : Profile
    {
        public PolizaProfile()
        {
            CreateMap<Poliza, PolizaDTO>();
        }
    }
    public class PolizaCreateProfile : Profile
    {
        public PolizaCreateProfile()
        {
            CreateMap<PolizaFormDTO, Poliza>();
        }
    }
    public class PolizaUpdateProfile : Profile
    {
        public PolizaUpdateProfile()
        {
            CreateMap<PolizaFormDTO, Poliza>();
        }
    }
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDTO, Usuario>();
        }
    }
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDTO>();
        }
    }
    public class ClienteCreateProfile : Profile
    {
        public ClienteCreateProfile()
        {
            CreateMap<ClienteFormDTO, Cliente>();
        }
    }
    public class ClienteUpdateProfile : Profile
    {
        public ClienteUpdateProfile()
        {
            CreateMap<ClienteFormDTO, Cliente>();
        }
    }
}