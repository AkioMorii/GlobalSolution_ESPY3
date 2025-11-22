using AutoMapper;
using GS2_Domain.Entities;
using GS2_API.DTOs.v1;
using System.Linq;

namespace GS2_API.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Entity -> UsuarioDto (listagem/detalhe)
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.TipoUsuarioDescricao,
                    opt => opt.MapFrom(src => src.TipoUsuario.Descricao))
                .ForMember(dest => dest.CursosCriados,
                    opt => opt.MapFrom(src => src.CursosCriados))
                .ForMember(dest => dest.MeusCursos,
                    opt => opt.MapFrom(src => src.MeusCursos));

            // Entity -> UsuarioResponseDto (retorno do login)
            CreateMap<Usuario, UsuarioResponseDto>()
                .ForMember(dest => dest.TipoUsuarioDescricao,
                    opt => opt.MapFrom(src => src.TipoUsuario.Descricao))
                .ForMember(dest => dest.MeusCursos,
                    opt => opt.MapFrom(src => src.MeusCursos));

            // Curso -> CursoResumoDto
            CreateMap<Curso, CursoResumoDto>()
                .ForMember(dest => dest.Nome,
                    opt => opt.MapFrom(src => src.Titulo));

            // MeusCursos -> MeusCursosDto
            CreateMap<MeusCursos, MeusCursosDto>()
                .ForMember(dest => dest.NomeCurso,
                    opt => opt.MapFrom(src => src.Curso.Titulo))
                .ForMember(dest => dest.DataInicio,
                    opt => opt.MapFrom(src => src.DataInicio))
                .ForMember(dest => dest.DataConclusao,
                    opt => opt.MapFrom(src => src.DataFim)); // mapear do campo do domínio DataFim -> DTO DataConclusao
        }
    }
}
