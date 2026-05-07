using System.Collections.Generic;
using System.Threading.Tasks;
using Gado.Dominio.Entidades;

namespace DataAccess.Repositorios; // Ajuste para o namespace que você usa nos outros repositórios

public interface IBaixaAnimalRepositorio
{
    Task<BaixaAnimal> SalvarAsync(BaixaAnimal baixa);
    Task<BaixaAnimal> ObterPorIdAsync(int id);
    Task<IEnumerable<BaixaAnimal>> ListarAsync(bool ativo);
    Task AtualizarAsync(BaixaAnimal baixa);
    Task DeletarAsync(BaixaAnimal baixa); // Nosso famoso Soft Delete
}