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
        }
    }
}
