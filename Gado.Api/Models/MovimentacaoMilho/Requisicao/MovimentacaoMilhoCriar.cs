using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores;

namespace Gado.Api.Models.Requisicao;

public class MovimentacaoMilhoCriar
{
    [Required(ErrorMessage = "É obrigatório informar o lote de milho (MilhoID).")]
    public int MilhoID { get; set; }

    [Required]
    public TipoMovimentacaoMilho Tipo { get; set; }

    [Required(ErrorMessage = "A quantidade em KG é obrigatória.")]
    public decimal QuantidadeKg { get; set; }

    public DateTime DataMovimentacao { get; set; }

    // --- CAMPOS OPCIONAIS (Usados apenas se for Venda) ---
    public decimal? ValorVenda { get; set; }
    public decimal? ValorPorSacoVendido { get; set; } // <--- NOVO CAMPO
    public FormaPagamento? Pagamento { get; set; }    // <--- NOVO CAMPO
    public string Comprador { get; set; }
    
    public string Observacao { get; set; }
}