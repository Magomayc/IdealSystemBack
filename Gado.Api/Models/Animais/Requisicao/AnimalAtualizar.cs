using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores; // <--- Importando o Enum

namespace Gado.Api.Models.Requisicao;

public class AnimalAtualizar
{
    [Required]
    public int ID { get; set; }

    [Required]
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