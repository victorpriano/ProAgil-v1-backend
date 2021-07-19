using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // N x N
            CreateMap<Evento, EventoDto>()
                    .ForMember(dest => dest.Palestrantes, opt => {
                        opt.MapFrom(src => src.PalestranteEventos.Select(x => x.Palestrante).ToList());
                    }).ReverseMap();
            
            // CreateMap<EventoDto, Evento>(); // reverseMap

            CreateMap<Palestrante, PalestranteDto>()
                    .ForMember(dest => dest.Eventos, opt => {
                        opt.MapFrom(src => src.PalestranteEventos.Select(x => x.Evento));
                    }).ReverseMap();
                    
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

        }
    }
}