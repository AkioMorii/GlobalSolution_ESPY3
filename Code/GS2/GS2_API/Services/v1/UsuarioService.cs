using AutoMapper;
using BCrypt.Net;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDto>> ListarAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(x => x.TipoUsuario)
                .Include(x => x.CursosCriados)
                .Include(x => x.MeusCursos)
                    .ThenInclude(mc => mc.Curso)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<UsuarioDto>>(usuarios);
        }

        public async Task<UsuarioDto?> ObterPorIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(x => x.TipoUsuario)
                .Include(x => x.CursosCriados)
                .Include(x => x.MeusCursos)
                    .ThenInclude(mc => mc.Curso)
                .FirstOrDefaultAsync(x => x.UsuarioId == id);

            return usuario == null ? null : _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto> CriarAsync(UsuarioCreateDto dto)
        {
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var novoUsuario = new Usuario(
                dto.Nome,
                dto.Email,
                dto.Login,
                senhaHash,
                dto.Telefone,
                dto.Cpf,
                dto.TipoUsuarioId
            );

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(novoUsuario);
        }

        public async Task<UsuarioDto?> AtualizarAsync(int id, UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (usuario == null) return null;

            // chama método de domínio seguro
            usuario.AtualizarDados(dto.Nome, dto.Email, dto.Telefone, dto.TipoUsuarioId, dto.Ativo);

            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> AlterarSenhaAsync(int id, UsuarioAlterarSenhaDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);
            if (usuario == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.SenhaHash))
                return false;

            string novaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

            usuario.DefinirNovaSenha(novaHash);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DesativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);
            if (usuario == null) return false;

            usuario.Desativar();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtivarAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);
            if (usuario == null) return false;

            usuario.Ativar();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }
       
    }
}
