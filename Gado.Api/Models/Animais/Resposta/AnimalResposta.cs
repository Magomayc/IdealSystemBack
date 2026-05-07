using Gado.Dominio.Enumeradores; // <--- Importando o Enum

namespace Gado.Api.Models.Resposta;

public class AnimalResposta
{
    public int Id { get; set; }
    public decimal Peso { get; set; } 
    public decimal ValorEntrada { get; set; }
    public string Vendedor { get; set; }
    public DateTime DataEntrada { get; set; }
    public bool Ativo { get; set; }

    // --- NOVOS CAMPOS QUE O FRONTEND VAI RECEBER ---
    public string Sexo { get; set; }
    public string Brinco { get; set; }
    public string Raca { get; set; }
    public decimal PrecoArroba { get; set; }
    
    // --- ESTOQUE ---
    public TipoEstoque Estoque { get; set; }
}