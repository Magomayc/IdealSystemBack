using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores;

namespace Gado.Api.Models.Requisicao;

public class MilhoCriar
{
    [Required(ErrorMessage = "O nome do Vendedor é obrigatório.")]
    public string Vendedor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A quantidade de sacos é obrigatória.")]
    [Range(1, 100000, ErrorMessage = "A quantidade de sacos deve ser maior que zero.")]
    public int QuantidadeSacos { get; set; }

    [Required(ErrorMessage = "O peso por saco é obrigatório.")]
    [Range(0.1, 1000, ErrorMessage = "O peso por saco deve ser maior que zero.")]
    public decimal PesoPorSaco { get; set; }

    [Required(ErrorMessage = "O valor por saco é obrigatório.")]
    [Range(0.01, 100000, ErrorMessage = "O valor por saco deve ser maior que zero.")]
    public decimal ValorPorSaco { get; set; }

    [Required(ErrorMessage = "A forma de pagamento é obrigatória.")]
    public FormaPagamento Pagamento { get; set; }

    [Required(ErrorMessage = "A data da compra é obrigatória.")]
    public DateTime DataCompra { get; set; }
}