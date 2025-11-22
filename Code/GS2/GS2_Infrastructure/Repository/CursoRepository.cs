using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_Infrastructure.Repository
{
    public class CursoRepository
    {
        private readonly AppDbContext _context;

        public CursoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Curso>> ListarAsync()
        {
            return await _context.Cursos
                .Include(c => c.Nivel)
                .Include(c => c.Proprietario)
                .Include(c => c.TrilhaAprendizagem)
                .Include(c => c.Conteudos)
                .Include(c => c.CursosPalavrasChave)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Curso?> ObterPorIdAsync(int id)
        {
            return await _context.Cursos
                .Include(c => c.Nivel)
                .Include(c => c.Proprietario)
                .Include(c => c.TrilhaAprendizagem)
                .Include(c => c.Conteudos)
                .Include(c => c.CursosPalavrasChave)
                .FirstOrDefaultAsync(c => c.CursoId == id);
        }

        public async Task CriarAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return false;

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
