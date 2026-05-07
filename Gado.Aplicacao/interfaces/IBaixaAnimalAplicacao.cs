using System.Collections.Generic;
using System.Threading.Tasks;
using Gado.Dominio.Entidades;
using Gado.Aplicacao.DTOs;

namespace Gado.Aplicacao;

public interface IBaixaAnimalAplicacao
{
    // Nomes 100% espelhados com a IVendaAplicacao
    Task<BaixaAnimal> CriarAsync(RegistrarBaixaDTO dto); 
    Task AtualizarAsync(AtualizarBaixaDTO dto);
    Task ExcluirAsync(int id); // Padronizado com Excluir (que faz o soft delete)
    Task<BaixaAnimal> ObterAsync(int id); 
    Task<IEnumerable<BaixaAnimal>> ListarAsync(bool ativo = true);
}