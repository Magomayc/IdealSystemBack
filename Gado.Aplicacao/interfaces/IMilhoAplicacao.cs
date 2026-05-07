using Gado.Dominio.Entidades;

namespace Gado.Aplicacao;

public interface IMilhoAplicacao
{
    Task<int> CriarAsync(Milho milho);
    Task AtualizarAsync(Milho milho);
    Task<Milho> ObterAsync(int id);
    Task DeletarAsync(int id);
    Task RestaurarAsync(int id);
    Task<IEnumerable<Milho>> ListarAsync(bool ativo, string termoBusca);
}