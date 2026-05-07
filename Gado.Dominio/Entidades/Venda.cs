using Gado.Dominio.Enumeradores; // Adicionamos o using aqui no topo!

namespace Gado.Dominio.Entidades;

public class Venda
{
    public int ID { get; set; }
    public DateTime DataVenda { get; set; }
    public string Comprador { get; set; } 
    
    // --- Usando o seu novo Enumerador ---
    public TipoBaixa Tipo { get; set; } = TipoBaixa.Venda; 
    
    public decimal PrecoArroba { get; set; } 
    public decimal ValorTotal { get; set; } 
    
    public bool Ativo { get; set; } 
    
    public virtual List<ItemVenda> Itens { get; set; }

    public Venda()
    {
        Itens = new List<ItemVenda>();
        Ativo = true;
        DataVenda = DateTime.Now;
        Tipo = TipoBaixa.Venda; // Mantém as antigas como Venda
    }

    public void CalcularTotalDaVenda()
    {
        if (Itens != null && Itens.Any())
        {
            ValorTotal = Itens.Sum(i => i.ValorUnitario);
        }
    }
}