using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;

namespace GS2_API.Mappings
{
    public class TipoConteudoProfile : Profile
    {
        public TipoConteudoProfile()
        {
            CreateMap<TipoConteudo, TipoConteudoDto>();
            // Caso precise de mapeamento reverso:
            // CreateMap<TipoConteudoDto, TipoConteudo>();
        }
    }
}
