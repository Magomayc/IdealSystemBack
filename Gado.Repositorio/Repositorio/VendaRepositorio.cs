using DataAccess.Contexto;
using Gado.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorios;

public class VendaRepositorio : BaseRepositorio, IVendaRepositorio
{
    public VendaRepositorio(GadoContexto contexto) : base(contexto)
    {
    }

    public async Task<Venda> SalvarAsync(Venda venda)
    {
        await _contexto.Vendas.AddAsync(venda);
        await _contexto.SaveChangesAsync();
        return venda;
    }

    public async Task AtualizarAsync(Venda venda)
    {
        _contexto.Vendas.Update(venda);
        await _contexto.SaveChangesAsync();
    }

    // Adicionamos o 'bool ativo' aqui para separar as vendas ativas das canceladas
    public async Task<IEnumerable<Venda>> ListarAsync(bool ativo, string queryComprador)
    {
        queryComprador = queryComprador?.Trim() ?? "";

        return await _contexto.Vendas
            .AsNoTracking()
            .Include(v => v.Itens) 
                .ThenInclude(i => i.Animal) 
            .Where(v => v.Ativo == ativo && EF.Functions.Like(v.Comprador, $"%{queryComprador}%"))
            .OrderByDescending(v => v.DataVenda) 
            .ToListAsync();
    }

    public async Task<Venda> ObterPorIdAsync(int id)
    {
        var venda = await _contexto.Vendas
            .Include(v => v.Itens) 
                .ThenInclude(i => i.Animal) 
            .FirstOrDefaultAsync(v => v.ID == id);

        if (venda == null)
            throw new Exception($"Venda com ID {id} não encontrada.");

        return venda;
    }

    // Transformamos o Deletar em Soft Delete (Cancelar Venda)
    public async Task DeletarAsync(Venda venda)
    {
        venda.Ativo = false; 
        _contexto.Vendas.Update(venda);
        await _contexto.SaveChangesAsync();
    }
}