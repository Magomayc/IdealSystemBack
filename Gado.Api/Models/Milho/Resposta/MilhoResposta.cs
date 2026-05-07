using Gado.Dominio.Enumeradores;

namespace Gado.Api.Models.Resposta;

public class MilhoResposta
{
    public int Id { get; set; }
    public DateTime DataCompra { get; set; }
    
    // 👇 Único ajuste: = string.Empty; para evitar avisos do compilador
    public string Vendedor { get; set; } = string.Empty;
    
    // --- CALCULADOS PELO BACKEND ---
    public decimal KgComprado { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal KgEstoqueAtual { get; set; }
    
    // --- DADOS DA SACARIA E FINANCEIRO ---
    public int QuantidadeSacos { get; set; }
    public decimal PesoPorSaco { get; set; } 
    public decimal ValorPorSaco { get; set; }
    public FormaPagamento Pagamento { get; set; }
    
    public bool Ativo { get; set; }
}