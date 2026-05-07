using DataAccess.Contexto;
using Gado.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorios;

public class UsuarioRepositorio : BaseRepositorio, IUsuarioRepositorio
{
    public UsuarioRepositorio(GadoContexto contexto) : base(contexto)
    {
    }

    public async Task<int> SalvarAsync(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();
        return usuario.ID;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo, string query)
    {
        query = query?.Trim() ?? "";

        return await _contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.Ativo == ativo && EF.Functions.Like(u.Nome, $"%{query}%"))
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Usuario> ObterPorIdAsync(int id)
    {
        var usuario = await _contexto.Usuarios.FindAsync(id);
        
        if (usuario == null)
            throw new Exception($"Usuário com ID {id} não encontrado.");

        return usuario;
    }

    public async Task<Usuario> ObterAsync(int usuarioId)
    {
        var usuario = await _contexto.Usuarios
            .Where(u => u.ID == usuarioId)
            .Where(u => u.Ativo)
            .FirstOrDefaultAsync();

        if (usuario == null)
            throw new Exception("Usuário ativo não encontrado.");

        return usuario;
    }

    public async Task<Usuario> ObterUsuarioDesativadoAsync(int usuarioId)
    {
        var usuario = await _contexto.Usuarios
            .Where(u => u.ID == usuarioId)
            .Where(u => !u.Ativo)
            .FirstOrDefaultAsync();

        if (usuario == null)
            throw new Exception("Usuário desativado não encontrado.");

        return usuario;
    }

    public async Task<Usuario> ObterPeloEmailAsync(string email)
    {
        var usuario = await _contexto.Usuarios
            .Where(u => u.Email == email)
            .Where(u => u.Ativo)
            .FirstOrDefaultAsync();

        if (usuario == null)
            throw new Exception($"Nenhum usuário ativo encontrado com o email {email}.");

        return usuario;
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        var usuario = await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
            
        if (usuario == null)
            throw new Exception($"Email {email} não cadastrado.");

        return usuario;
    }
}