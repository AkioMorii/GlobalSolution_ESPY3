using AutoMapper;
using GS2_Domain.Entities;
using GS2_API.DTOs.v1;

namespace GS2_API.Mappings
{
    public class CursoProfile : Profile
    {
        public CursoProfile()
        {
            CreateMap<Curso, CursoDto>()
                .ForMember(dest => dest.NivelNome,
                    opt => opt.MapFrom(src => src.Nivel.Descricao))
                .ForMember(dest => dest.ProprietarioNome,
                    opt => opt.MapFrom(src => src.Proprietario.Nome))
                .ForMember(dest => dest.TrilhaTitulo,
                    opt => opt.MapFrom(src => src.TrilhaAprendizagem.Titulo))
                .ReverseMap()
                .ForMember(dest => dest.Nivel, opt => opt.Ignore())
                .ForMember(dest => dest.Proprietario, opt => opt.Ignore())
                .ForMember(dest => dest.TrilhaAprendizagem, opt => opt.Ignore());
        }
    }
}