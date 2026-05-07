using Gado.Dominio.Entidades;
using Gado.Aplicacao.DTOs; 


namespace Gado.Aplicacao;

public interface IVendaAplicacao
{
    // Recebe o DTO com a lista de bois e pesos vivos
    Task<Venda> CriarAsync(RegistrarVendaDTO dto);
    
    // Atualiza a nota fiscal preenchendo o Peso Morto que veio do frigorífico
    Task AtualizarRomaneioAsync(AtualizarVendaDTO dto);
    
    Task<Venda> ObterAsync(int id);
    
    // Soft Delete (Devolve o boi pro pasto e cancela a venda)
    Task ExcluirAsync(int id); 
    
    // Adicionado o 'bool ativo' para filtrar vendas válidas x canceladas
    Task<IEnumerable<Venda>> ListarAsync(bool ativo, string queryComprador);
}