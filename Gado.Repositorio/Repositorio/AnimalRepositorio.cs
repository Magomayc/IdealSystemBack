using DataAccess.Contexto;
using Gado.Dominio.Entidades;
using Gado.Dominio.Enumeradores; // <--- Referência para o seu Enum
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorios;

public class AnimalRepositorio : BaseRepositorio, IAnimalRepositorio
{
    public AnimalRepositorio(GadoContexto contexto) : base(contexto)
    {
    }

    public async Task<int> SalvarAsync(Animal animal)
    {
        await _contexto.Animais.AddAsync(animal);
        await _contexto.SaveChangesAsync();
        return animal.ID;
    }

    public async Task AtualizarAsync(Animal animal)
    {
        _contexto.Animais.Update(animal);
        await _contexto.SaveChangesAsync();
    }

    // --- NOVO PARÂMETRO OPCIONAL DE ESTOQUE ---
    public async Task<IEnumerable<Animal>> ListarAsync(bool ativo, string queryVendedor, TipoEstoque? tipoEstoque = null)
    {
        var termoBusca = queryVendedor?.Trim() ?? "";

        return await _contexto.Animais
            .AsNoTracking()
            .Where(a => a.Ativo == ativo && 
                        (!tipoEstoque.HasValue || a.Estoque == tipoEstoque) && // <--- FILTRO INTELIGENTE DE ESTOQUE
                       (EF.Functions.Like(a.Vendedor, $"%{termoBusca}%") || 
                        EF.Functions.Like(a.Brinco, $"%{termoBusca}%") || 
                        EF.Functions.Like(a.Raca, $"%{termoBusca}%")))
            .OrderByDescending(a => a.DataEntrada) // Ordena pelos mais recentes
            .ToListAsync();
    }

    public async Task<Animal> ObterPorIdAsync(int id)
    {
        var animal = await _contexto.Animais.FindAsync(id);

        if (animal == null)
            throw new Exception($"Animal com ID {id} não encontrado.");

        return animal;
    }
}