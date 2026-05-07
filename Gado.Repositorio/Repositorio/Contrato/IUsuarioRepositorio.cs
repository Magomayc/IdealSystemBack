using Gado.Dominio.Entidades;

namespace DataAccess.Repositorios;

public interface IUsuarioRepositorio
{
    Task AtualizarAsync(Usuario usuario);

    Task<IEnumerable<Usuario>> ListarAsync(bool ativo, string query);

    Task<Usuario> ObterAsync(int usuarioId);

    Task<Usuario> ObterUsuarioDesativadoAsync(int usuarioId);

    Task<Usuario> ObterPeloEmailAsync(string email);
    
    Task<int> SalvarAsync(Usuario usuario);

    Task<Usuario> ObterPorEmailAsync(string email);
    
    Task<Usuario> ObterPorIdAsync(int id);
}