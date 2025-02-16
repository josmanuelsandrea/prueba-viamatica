using AutoMapper;
using viamatica_backend.DBModels;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;

namespace viamatica_backend.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Persona, opt => opt.MapFrom(src => src.IdPersonaNavigation));
            CreateMap<PersonaRequest, Persona>();
            CreateMap<RolUsuario, RoleUserDTO>();
            CreateMap<RolOpcione, OpcionesDTO>();
            CreateMap<Rol, RoleDTO>();
            CreateMap<Persona, PersonaDTO>();
            CreateMap<HistorialSesione, HistorialSesioneDTO>();
        }
    }
}
