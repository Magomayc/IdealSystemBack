using Gado.Dominio.Entidades;

namespace Gado.Aplicacao;

public interface IUsuarioAplicacao
{
    Task<int> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task AtualizarSenhaAsync(Usuario usuario, string senhaAntiga);
    Task<Usuario> ObterAsync(int usuarioId);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task DeletarAsync(int usuarioId);
    Task RestaurarAsync(int usuarioId);
    Task<IEnumerable<Usuario>> ListarAsync(bool ativo, string query);
    IEnumerable<object> ListarTiposUsuario();
    Task<Usuario> LoginAsync(string email, string senha);
}