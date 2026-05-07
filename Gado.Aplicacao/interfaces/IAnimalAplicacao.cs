using Gado.Dominio.Entidades;
using Gado.Dominio.Enumeradores; // <--- Adicionamos a referência ao Enum

namespace Gado.Aplicacao;

public interface IAnimalAplicacao
{
    Task<int> CriarAsync(Animal animal);
    Task AtualizarAsync(Animal animal);
    Task<Animal> ObterAsync(int id);
    Task DeletarAsync(int id);
    Task RestaurarAsync(int id);
    
    // --- NOVO PARÂMETRO OPCIONAL DE ESTOQUE ---
    Task<IEnumerable<Animal>> ListarAsync(bool ativo, string termoBusca, TipoEstoque? tipoEstoque = null);
}