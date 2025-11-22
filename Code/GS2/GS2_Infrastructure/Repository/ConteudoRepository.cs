using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_Infrastructure.Repository
{
    public class ConteudoRepository
    {
        private readonly AppDbContext _context;

        public ConteudoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Conteudo>> ListarAsync()
        {
            return await _context.Conteudos
                .Include(c => c.TipoConteudo)
                .ToListAsync();
        }

        public async Task<Conteudo?> ObterPorIdAsync(int id)
        {
            return await _context.Conteudos
                .Include(c => c.TipoConteudo)
                .FirstOrDefaultAsync(c => c.ConteudoId == id);
        }

        public async Task CriarAsync(Conteudo conteudo)
        {
            _context.Conteudos.Add(conteudo);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Conteudo conteudo)
        {
            _context.Conteudos.Update(conteudo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var c = await _context.Conteudos.FirstOrDefaultAsync(x => x.ConteudoId == id);
            if (c == null) return false;

            _context.Conteudos.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
