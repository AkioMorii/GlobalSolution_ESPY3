using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_Infrastructure.Repository
{
    public class CursoPalavraChaveRepository
    {
        private readonly AppDbContext _context;

        public CursoPalavraChaveRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CursoPalavraChave>> ListarPorCursoAsync(int cursoId)
        {
            return await _context.CursoPalavraChave
                .Include(cp => cp.PalavraChave)
                .Where(cp => cp.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task AdicionarAsync(CursoPalavraChave item)
        {
            _context.CursoPalavraChave.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int cursoId, int palavraChaveId)
        {
            var entity = await _context.CursoPalavraChave
                .FirstOrDefaultAsync(cp => cp.CursoId == cursoId && cp.PalavraChaveId == palavraChaveId);
            if (entity != null)
            {
                _context.CursoPalavraChave.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
