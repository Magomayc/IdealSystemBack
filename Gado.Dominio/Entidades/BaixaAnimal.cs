using System;

namespace Gado.Dominio.Entidades;

public class BaixaAnimal
{
    public int ID { get; set; }
    
    // Quando aconteceu a perda
    public DateTime DataBaixa { get; set; }
    
    // Qual boi sofrou a baixa
    public int AnimalID { get; set; }
    public virtual Animal Animal { get; set; }
    
    // Categoria rápida para relatórios (Ex: "Morte Natural", "Raio", "Roubo")
    public string Motivo { get; set; } 
    
    // Detalhes extras ("Encontrado perto da cerca sul...")
    public string Observacao { get; set; } 
    
    // Para podermos estornar caso o peão dê baixa no boi errado
    public bool Ativo { get; set; }

    public BaixaAnimal()
    {
        DataBaixa = DateTime.Now;
        Ativo = true;
    }
}