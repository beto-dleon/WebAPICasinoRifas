using AutoMapper;
using WebAPICasinoRifas.DTOs;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PremioCreacionDTO, Premio>();
            CreateMap<ParticipanteCreacionDTO, Participante>();
            CreateMap<RifaConParticipanteCreacionDTO, RifaConParticipante>();
            CreateMap<RifaCreacionDTO, Rifa>();
            
            CreateMap<Rifa, GetRifaDTO>().ForMember(premioDTO => premioDTO.Premios, options => options.MapFrom(MapearListaPremios));

        }

        private List<Premio> MapearListaPremios(Rifa rifa, GetRifaDTO getRifaDTO)
        {
            var result = new List<Premio>();
            if (rifa.Premios == null)
            {
                return result;
            }

            foreach(var premios in rifa.Premios)
            {
                result.Add(new Premio()
                {
                    Id = premios.Id,
                    Nombre = premios.Nombre,
                    Disponibilidad = premios.Disponibilidad,
                    Orden = premios.Orden,
                    RifaId = premios.RifaId
                });
            } 
            return result;
        }

    }
}
