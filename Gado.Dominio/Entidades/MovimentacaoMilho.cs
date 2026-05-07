using Gado.Dominio.Enumeradores;

namespace Gado.Dominio.Entidades;

public class MovimentacaoMilho
{
    public int ID { get; set; }
    public int MilhoID { get; set; } 
    public virtual Milho Milho { get; set; } 
    
    public TipoMovimentacaoMilho Tipo { get; set; }
    public decimal QuantidadeKg { get; set; }
    public DateTime DataMovimentacao { get; set; }
    
    // --- CAMPOS DE VENDA (Opcionais) ---
    public decimal? ValorVenda { get; set; } 
    public decimal? ValorPorSacoVendido { get; set; } // <--- NOVO
    public FormaPagamento? Pagamento { get; set; } // <--- NOVO
    public string Comprador { get; set; }
    
    // --- INTELIGÊNCIA DE CUSTO ---
    public decimal CustoMovimentacao { get; set; } 
    public string Observacao { get; set; } 
    public bool Ativo { get; set; }

    public MovimentacaoMilho()
    {
        Ativo = true;
        DataMovimentacao = DateTime.Now;
    }

    public void Deletar() { Ativo = false; }
    public void Restaurar() { Ativo = true; }

    public void CalcularFinanceiro(Milho milhoOrigem)
    {
        decimal custoPorKg = milhoOrigem.ValorTotal / milhoOrigem.KgComprado;
        CustoMovimentacao = Math.Round(QuantidadeKg * custoPorKg, 2);

        // Limpa a sujeira: Se não for venda, garante que não tenha dados financeiros de terceiros perdidos
        if (Tipo == TipoMovimentacaoMilho.Consumo || Tipo == TipoMovimentacaoMilho.Perda)
        {
            ValorVenda = null;
            ValorPorSacoVendido = null; // <--- NOVO
            Pagamento = null; // <--- NOVO
            Comprador = null;
        }
    }
}