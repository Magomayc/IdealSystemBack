using Gado.Dominio.Entidades;

namespace DataAccess.Repositorios;

public interface IMovimentacaoMilhoRepositorio
{
    Task<int> SalvarAsync(MovimentacaoMilho movimentacao);
    Task AtualizarAsync(MovimentacaoMilho movimentacao);
    Task<MovimentacaoMilho> ObterPorIdAsync(int id);
    
    // milhoId opcional: se passar, lista só as movimentações daquela compra específica
    Task<IEnumerable<MovimentacaoMilho>> ListarAsync(bool ativo, int? milhoId = null); 
}