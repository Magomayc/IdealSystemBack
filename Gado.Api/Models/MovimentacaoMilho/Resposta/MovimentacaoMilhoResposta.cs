using Gado.Dominio.Enumeradores;

namespace Gado.Api.Models.Resposta;

public class MovimentacaoMilhoResposta
{
    public int Id { get; set; }
    public int MilhoId { get; set; }
    
    public TipoMovimentacaoMilho Tipo { get; set; }
    public decimal QuantidadeKg { get; set; }
    public DateTime DataMovimentacao { get; set; }
    
    // --- FINANCEIRO DA VENDA ---
    public decimal? ValorVenda { get; set; }
    public decimal? ValorPorSacoVendido { get; set; } // <--- NOVO CAMPO
    public FormaPagamento? Pagamento { get; set; }    // <--- NOVO CAMPO
    public string Comprador { get; set; }
    
    // --- CUSTO CALCULADO ---
    public decimal CustoMovimentacao { get; set; } 
    
    public string Observacao { get; set; }
    public bool Ativo { get; set; }
}