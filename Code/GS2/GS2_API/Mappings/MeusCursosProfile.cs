using AutoMapper;
using GS2_Domain.Entities;
using GS2_API.DTOs.v1;

namespace GS2_API.Mappings
{
    public class MeusCursosProfile : Profile
    {
        public MeusCursosProfile()
        {
            // ENTIDADE → DTO
            CreateMap<MeusCursos, MeusCursosDto>()
                .ForMember(dest => dest.CursoTitulo,
                    opt => opt.MapFrom(src => src.Curso.Titulo));

            // DTO PARA CRIAÇÃO → ENTIDADE
            CreateMap<MeusCursosCreateDto, MeusCursos>()
                .ForMember(dest => dest.DataInicio, opt => opt.Ignore())
                .ForMember(dest => dest.DataFim, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            // DTO PARA CONCLUSÃO → ENTIDADE (apenas chave para localizar)
            CreateMap<MeusCursosConclusaoDto, MeusCursos>()
                .ForMember(dest => dest.DataInicio, opt => opt.Ignore())
                .ForMember(dest => dest.DataFim, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());
        }
    }
}
