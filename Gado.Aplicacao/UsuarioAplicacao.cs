using Gado.Dominio.Entidades;
using Gado.Dominio.Enumeradores;
using DataAccess.Repositorios; 

namespace Gado.Aplicacao;

public class UsuarioAplicacao : IUsuarioAplicacao
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new Exception("Usuário não pode ser vazio.");

        ValidarInfomacoesUsuario(usuario);

        if (string.IsNullOrEmpty(usuario.Senha))
            throw new Exception("Senha não pode ser vazia.");
            
        try 
        {
            var usuarioExistente = await _usuarioRepositorio.ObterPeloEmailAsync(usuario.Email);
            if (usuarioExistente != null)
            {
                throw new Exception($"O e-mail {usuario.Email} já está em uso.");
            }
        }
        catch (Exception ex) 
        {
            if (ex.Message.Contains("já está em uso")) throw;
        }

        return await _usuarioRepositorio.SalvarAsync(usuario);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuario.ID);

        ValidarInfomacoesUsuario(usuario);

        usuarioDominio.Nome = usuario.Nome;
        usuarioDominio.Email = usuario.Email;
        usuarioDominio.TipoUsuarioID = usuario.TipoUsuarioID;

        // 👇 A MÁGICA DA SENHA AQUI: Só altera a senha no banco se o usuário digitou uma nova!
        if (!string.IsNullOrWhiteSpace(usuario.Senha))
        {
            usuarioDominio.Senha = usuario.Senha;
        }

        await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
    }

    public async Task AtualizarSenhaAsync(Usuario usuario, string senhaAntiga)
    {
        var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuario.ID);

        if (usuarioDominio.Senha != senhaAntiga)
        {
            throw new Exception("A Senha Antiga informada é inválida.");
        }

        if (string.IsNullOrEmpty(usuario.Senha))
        {
            throw new Exception("A nova senha não pode ser vazia.");
        }

        usuarioDominio.Senha = usuario.Senha;

        await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
    }

    public async Task<Usuario> ObterAsync(int usuarioId)
    {
        return await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _usuarioRepositorio.ObterPeloEmailAsync(email);
    }

    public async Task DeletarAsync(int usuarioId)
    {
        var usuarioDominio = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
        
        usuarioDominio.Deletar(); // Soft Delete

        await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
    }

    public async Task RestaurarAsync(int usuarioId)
    {
        var usuarioDominio = await _usuarioRepositorio.ObterUsuarioDesativadoAsync(usuarioId);

        usuarioDominio.Restaurar();

        await _usuarioRepositorio.AtualizarAsync(usuarioDominio);
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(bool ativo, string query)
    {
        query = query?.Trim() ?? "";
        return await _usuarioRepositorio.ListarAsync(ativo, query);
    }

    public IEnumerable<object> ListarTiposUsuario()
    {
        var nomesEnum = Enum.GetNames(typeof(TipoUsuarios));
        var valoresEnum = (int[])Enum.GetValues(typeof(TipoUsuarios));

        List<object> objetosTipoUsuarioEnum = new List<object>();

        for (int i = 0; i < valoresEnum.Length; i++)
        {
            objetosTipoUsuarioEnum.Add(new
            {
                id = valoresEnum[i],
                nome = nomesEnum[i]
            });
        }

        return objetosTipoUsuarioEnum;
    }

    public async Task<Usuario> LoginAsync(string email, string senha)
    {
        Usuario usuario;
        try 
        {
             usuario = await _usuarioRepositorio.ObterPeloEmailAsync(email);
        }
        catch
        {
             throw new Exception("E-mail ou senha inválidos.");
        }

        if (usuario == null || usuario.Senha != senha)
        {
            throw new Exception("E-mail ou senha inválidos.");
        }

        return usuario;
    }

    #region Util

    private static void ValidarInfomacoesUsuario(Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Nome))
        {
            throw new Exception("Nome não pode ser vazio.");
        }

        if (string.IsNullOrEmpty(usuario.Email))
        {
            throw new Exception("E-mail não pode ser vazio.");
        }

        var valoresEnum = (int[])Enum.GetValues(typeof(TipoUsuarios));

        if (!valoresEnum.Contains((int)usuario.TipoUsuarioID))
        {
            throw new Exception("Tipo de Usuário inválido.");
        }
    }

    #endregion
}