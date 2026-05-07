using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores; // <--- Importando o Enum

namespace Gado.Api.Models.Requisicao;

public class AnimalCriar
{
    [Required(ErrorMessage = "O peso é obrigatório")]
    // Mudamos para decimal para bater com o Banco de Dados
    public decimal Peso { get; set; } 

    [Required]
    public decimal ValorEntrada { get; set; }

    [Required]
    public string Vendedor { get; set; }

    public DateTime DataEntrada { get; set; }

    // --- NOVOS CAMPOS ---
    public string Sexo { get; set; }
    public string Brinco { get; set; }
    public string Raca { get; set; }
    public decimal PrecoArroba { get; set; }
    
    // --- ESTOQUE ---
    public TipoEstoque Estoque { get; set; }
}