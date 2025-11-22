using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class TipoUsuarioService
    {
        private readonly AppDbContext _context;

        public TipoUsuarioService(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos
        public async Task<List<TipoUsuario>> ListarAsync()
        {
            return await _context.TipoUsuarios.ToListAsync();
        }

        // Buscar por ID
        public async Task<TipoUsuario?> ObterPorIdAsync(int id)
        {
            return await _context.TipoUsuarios.FindAsync(id);
        }

        // Buscar por Descrição (contains, case insensitive)
        public async Task<List<TipoUsuario>> BuscarPorDescricaoAsync(string descricao)
        {
            return await _context.TipoUsuarios
                .Where(t => t.Descricao.ToLower().Contains(descricao.ToLower()))
                .ToListAsync();
        }

        // Criar
        public async Task<TipoUsuario> CriarAsync(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuarios.Add(tipoUsuario);
            await _context.SaveChangesAsync();
            return tipoUsuario;
        }

        // Atualizar
        public async Task AtualizarAsync(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuarios.Update(tipoUsuario);
            await _context.SaveChangesAsync();
        }

        // Deletar
        public async Task DeletarAsync(int id)
        {
            var entity = await _context.TipoUsuarios.FindAsync(id);
            if (entity != null)
            {
                _context.TipoUsuarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
