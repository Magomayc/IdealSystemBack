using Gado.Dominio.Entidades;

namespace DataAccess.Repositorios;

public interface IMilhoRepositorio
{
    Task<int> SalvarAsync(Milho milho);
    Task AtualizarAsync(Milho milho);
    Task<Milho> ObterPorIdAsync(int id);
    Task<IEnumerable<Milho>> ListarAsync(bool ativo, string termoBusca);
}