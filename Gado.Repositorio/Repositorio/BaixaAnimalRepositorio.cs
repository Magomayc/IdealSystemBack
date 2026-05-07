using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gado.Dominio.Entidades;
using DataAccess.Contexto; 

namespace DataAccess.Repositorios; 

public class BaixaAnimalRepositorio : IBaixaAnimalRepositorio
{
    private readonly GadoContexto _contexto;

    public BaixaAnimalRepositorio(GadoContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<BaixaAnimal> SalvarAsync(BaixaAnimal baixa)
    {
        await _contexto.BaixasAnimais.AddAsync(baixa);
        await _contexto.SaveChangesAsync();
        return baixa;
    }

    public async Task<BaixaAnimal> ObterPorIdAsync(int id)
    {
        // O Include traz a ficha do boi junto com o motivo da morte
        return await _contexto.BaixasAnimais
            .Include(b => b.Animal) 
            .FirstOrDefaultAsync(b => b.ID == id);
    }

    public async Task<IEnumerable<BaixaAnimal>> ListarAsync(bool ativo)
    {
        return await _contexto.BaixasAnimais
            .Include(b => b.Animal)
            .Where(b => b.Ativo == ativo)
            .OrderByDescending(b => b.DataBaixa) // Mostra as mais recentes primeiro
            .ToListAsync();
    }

    public async Task AtualizarAsync(BaixaAnimal baixa)
    {
        _contexto.BaixasAnimais.Update(baixa);
        await _contexto.SaveChangesAsync();
    }

    public async Task DeletarAsync(BaixaAnimal baixa)
    {
        // Cancelamento da Baixa (Soft Delete)
        baixa.Ativo = false;
        _contexto.BaixasAnimais.Update(baixa);
        await _contexto.SaveChangesAsync();
    }
}