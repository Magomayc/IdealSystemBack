using Gado.Dominio.Entidades;
using Gado.Dominio.Enumeradores; 

namespace DataAccess.Repositorios;

public interface IAnimalRepositorio
{
    Task<int> SalvarAsync(Animal animal);
    Task AtualizarAsync(Animal animal);
    
    // --- NOVO PARÂMETRO OPCIONAL DE ESTOQUE ---
    Task<IEnumerable<Animal>> ListarAsync(bool ativo, string termoBusca, TipoEstoque? tipoEstoque = null);
    
    Task<Animal> ObterPorIdAsync(int id);
}