using Gado.Dominio.Entidades;

namespace DataAccess.Repositorios;

public interface IVendaRepositorio
{
    Task<Venda> SalvarAsync(Venda venda);

    Task AtualizarAsync(Venda venda);

    Task<IEnumerable<Venda>> ListarAsync(bool ativo, string queryComprador);

    Task<Venda> ObterPorIdAsync(int id);

    Task DeletarAsync(Venda venda);
}