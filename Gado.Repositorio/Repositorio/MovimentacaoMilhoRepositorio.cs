using DataAccess.Contexto;
using Gado.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorios;

public class MovimentacaoMilhoRepositorio : BaseRepositorio, IMovimentacaoMilhoRepositorio
{
    public MovimentacaoMilhoRepositorio(GadoContexto contexto) : base(contexto)
    {
    }

    public async Task<int> SalvarAsync(MovimentacaoMilho movimentacao)
    {
        await _contexto.MovimentacoesMilho.AddAsync(movimentacao);
        await _contexto.SaveChangesAsync();
        return movimentacao.ID;
    }

    public async Task AtualizarAsync(MovimentacaoMilho movimentacao)
    {
        _contexto.MovimentacoesMilho.Update(movimentacao);
        await _contexto.SaveChangesAsync();
    }

    public async Task<MovimentacaoMilho> ObterPorIdAsync(int id)
    {
        var movimentacao = await _contexto.MovimentacoesMilho
                                          .Include(m => m.Milho) // Traz os dados da compra original junto
                                          .FirstOrDefaultAsync(m => m.ID == id);

        if (movimentacao == null)
            throw new Exception($"Movimentação com ID {id} não encontrada.");

        return movimentacao;
    }

    public async Task<IEnumerable<MovimentacaoMilho>> ListarAsync(bool ativo, int? milhoId = null)
    {
        var query = _contexto.MovimentacoesMilho
                             .Include(m => m.Milho)
                             .AsNoTracking()
                             .Where(m => m.Ativo == ativo);

        // Se o Front-end quiser ver o histórico de um lote específico, a gente filtra aqui:
        if (milhoId.HasValue && milhoId.Value > 0)
        {
            query = query.Where(m => m.MilhoID == milhoId.Value);
        }

        return await query.OrderByDescending(m => m.DataMovimentacao).ToListAsync();
    }
}