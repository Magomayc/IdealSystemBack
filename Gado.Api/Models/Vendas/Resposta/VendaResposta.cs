using System;
using System.Collections.Generic;
using Gado.Dominio.Enumeradores; // AJUSTE 1: Importamos o seu enumerador

namespace Gado.Api.Models.Resposta;

public class VendaResposta
{
    public int Id { get; set; }
    
    // AJUSTE 2: Devolvemos o Tipo (1 = Venda, 2 = Baixa) para o React conseguir filtrar os relatórios!
    public TipoBaixa Tipo { get; set; } 
    
    public string Comprador { get; set; }
    public DateTime DataVenda { get; set; }

    // --- DADOS FINANCEIROS DA NOTA ---
    public decimal PrecoArroba { get; set; }
    public decimal ValorTotal { get; set; } // A soma de tudo, que veio cravada do front
    public bool Ativo { get; set; } // Para o Front saber se a venda foi cancelada/estornada

    // --- LISTA DE BOIS VENDIDOS ---
    public List<ItemVendaResposta> Itens { get; set; } = new List<ItemVendaResposta>();
}

// Classe que representa cada linha da nota fiscal (cada boi)
public class ItemVendaResposta
{
    public int AnimalID { get; set; }
    
    // Mantivemos a sua excelente ideia de devolver o objeto Animal completo!
    public AnimalResposta Animal { get; set; } 

    // --- BALANÇA E FRIGORÍFICO ---
    public decimal PesoVivo { get; set; } // O peso da balança no dia do embarque
    public decimal? PesoMorto { get; set; }
    
    // --- MATEMÁTICA QUE O NOSSO C# FEZ ---
    public decimal? RendimentoCarcaca { get; set; }
    public decimal? TotalArrobas { get; set; }
    public decimal ValorUnitario { get; set; } // Quanto de dinheiro esse boi específico gerou

    // --- SNAPSHOT (FOTOGRAFIA DO PASSADO) ---
    // O C# vai enviar para o React exatamente como o animal era no dia em que foi vendido!
    public string Raca { get; set; } 
    public decimal PesoEntrada { get; set; } 
    public decimal ValorEntrada { get; set; } 
}