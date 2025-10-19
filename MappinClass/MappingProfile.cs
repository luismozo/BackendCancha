using AutoMapper;
using BackendCancha.Data;
using BackendCancha.Model;
using BackendCancha.DTO;

namespace BackendCancha.MappinClass
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<Productos, ProductosDTO>();
            CreateMap<Usuario,UsuarioDTO>();
            CreateMap<Canchas, CanchasDTO>();
        }
    }
}
