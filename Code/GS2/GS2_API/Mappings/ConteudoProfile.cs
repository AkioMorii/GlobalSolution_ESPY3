using AutoMapper;
using GS2_Domain.Entities;
using GS2_API.DTOs.v1;

namespace GS2_API.Mappings
{
    public class ConteudoProfile : Profile
    {
        public ConteudoProfile()
        {
            CreateMap<Conteudo, ConteudoDto>();
        }
    }
}
