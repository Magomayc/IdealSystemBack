using DataAccess.Contexto;
using Gado.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorios;

public class MilhoRepositorio : BaseRepositorio, IMilhoRepositorio
{
    public MilhoRepositorio(GadoContexto contexto) : base(contexto)
    {
    }

    public async Task<int> SalvarAsync(Milho milho)
    {
        await _contexto.Milhos.AddAsync(milho);
        await _contexto.SaveChangesAsync();
        return milho.ID;
    }

    public async Task AtualizarAsync(Milho milho)
    {
        _contexto.Milhos.Update(milho);
        await _contexto.SaveChangesAsync();
    }

    public async Task<Milho> ObterPorIdAsync(int id)
    {
        var milho = await _contexto.Milhos.FindAsync(id);

        if (milho == null)
            throw new Exception($"Compra de milho com ID {id} não encontrada.");

        return milho;
    }

    public async Task<IEnumerable<Milho>> ListarAsync(bool ativo, string termoBusca)
    {
        var busca = termoBusca?.Trim() ?? "";

        return await _contexto.Milhos
            .AsNoTracking()
            .Where(m => m.Ativo == ativo && 
                       (EF.Functions.Like(m.Vendedor, $"%{busca}%")))
            .OrderByDescending(m => m.DataCompra) // Traz as compras mais recentes primeiro
            .ToListAsync();
    }
}