using Gado.Dominio.Enumeradores;

namespace Gado.Dominio.Entidades;

public class Animal
{
    public int ID { get; set; }
    public DateTime DataEntrada { get; set; }
    public decimal Peso { get; set; }
    public decimal ValorEntrada { get; set; }
    public string Vendedor { get; set; }
    public bool Ativo { get; set; }
    public string Sexo { get; set; }
    public string Brinco { get; set; }
    public string Raca { get; set; }
    public decimal PrecoArroba { get; set; }
    public TipoEstoque Estoque { get; set; } = TipoEstoque.Pasto;

    public Animal()
    {
        Ativo = true;
    }

    public void Deletar()
    {
        Ativo = false;
    }

    public void Restaurar()
    {
        Ativo = true;
    }
}