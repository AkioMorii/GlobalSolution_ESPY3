using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;

namespace GS2_API.Mappings
{
    public class NivelProfile : Profile
    {
        public NivelProfile()
        {
            CreateMap<Nivel, NivelDto>().ReverseMap();
        }
    }
}
