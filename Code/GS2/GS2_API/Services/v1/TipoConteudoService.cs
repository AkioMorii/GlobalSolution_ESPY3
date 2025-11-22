using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Domain.Exceptions;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class TiposConteudoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TiposConteudoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TipoConteudoDto> CriarAsync(TipoConteudoDto dto)
        {
            var tipo = _mapper.Map<TipoConteudo>(dto);

            _context.TiposConteudo.Add(tipo);
            await _context.SaveChangesAsync();

            return _mapper.Map<TipoConteudoDto>(tipo);
        }
        public async Task<TipoConteudoDto> ObterPorIdAsync(int id)
        {
            var tipo = await _context.TiposConteudo.FindAsync(id);
            if (tipo == null)
                throw new KeyNotFoundException($"TipoConteudo não encontrado: {id}");

            return _mapper.Map<TipoConteudoDto>(tipo);
        }

        public async Task<IEnumerable<TipoConteudoDto>> BuscarAsync(string? descricao = null)
        {
            var query = _context.TiposConteudo.AsQueryable();

            if (!string.IsNullOrWhiteSpace(descricao))
                query = query.Where(x => x.Descricao.Contains(descricao));

            var lista = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TipoConteudoDto>>(lista);
        }
        public async Task<TipoConteudoDto> AtualizarAsync(int id, TipoConteudoDto dto)
        {
            var tipo = await _context.TiposConteudo.FindAsync(id);
            if (tipo == null)
                throw new KeyNotFoundException($"TipoConteudo não encontrado: {id}");

            _mapper.Map(dto, tipo);
            await _context.SaveChangesAsync();

            return _mapper.Map<TipoConteudoDto>(tipo);
        }
        public async Task ExcluirAsync(int id)
        {
            var tipo = await _context.TiposConteudo.FindAsync(id);
            if (tipo == null)
                throw new KeyNotFoundException($"TipoConteudo não encontrado: {id}");

            _context.TiposConteudo.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }
}