using Gado.Dominio.Enumeradores;

namespace Gado.Dominio.Entidades;

public class Milho
{
    public int ID { get; set; }
    public DateTime DataCompra { get; set; }
    public string Vendedor { get; set; }
    
    // --- INFORMAÇÕES DE VOLUME ---
    public decimal KgComprado { get; set; }
    public int QuantidadeSacos { get; set; } 
    public decimal PesoPorSaco { get; set; } // <--- NOVO CAMPO AQUI
    
    // --- INFORMAÇÕES FINANCEIRAS ---
    public decimal ValorPorSaco { get; set; }
    public decimal ValorTotal { get; set; }
    public FormaPagamento Pagamento { get; set; } 

    // --- CONTROLE DE ESTOQUE ---
    public decimal KgEstoqueAtual { get; set; } 

    public bool Ativo { get; set; }

    public Milho()
    {
        Ativo = true;
        DataCompra = DateTime.Now;
    }

    public void Deletar() { Ativo = false; }
    public void Restaurar() { Ativo = true; }

    public void CalcularTotais()
    {
        // <--- CÁLCULO DINÂMICO AGORA ---
        KgComprado = QuantidadeSacos * PesoPorSaco; 
        ValorTotal = QuantidadeSacos * ValorPorSaco;
        
        if (KgEstoqueAtual == 0) 
        {
            KgEstoqueAtual = KgComprado;
        }
    }
}