using Gado.Dominio.Entidades;

namespace Gado.Aplicacao;

public interface IMovimentacaoMilhoAplicacao
{
    Task<int> CriarAsync(MovimentacaoMilho movimentacao);
    Task AtualizarAsync(MovimentacaoMilho movimentacao);
    Task<MovimentacaoMilho> ObterAsync(int id);
    Task DeletarAsync(int id);
    Task RestaurarAsync(int id);
    Task<IEnumerable<MovimentacaoMilho>> ListarAsync(bool ativo, int? milhoId = null);
}