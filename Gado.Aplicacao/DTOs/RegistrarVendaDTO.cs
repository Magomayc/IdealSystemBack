using System;
using System.Collections.Generic;
using Gado.Dominio.Enumeradores; // AJUSTE 1: Importamos o enumerador para o DTO

namespace Gado.Aplicacao.DTOs;

public class RegistrarVendaDTO
{
    public string Comprador { get; set; }
    public DateTime DataVenda { get; set; }
    
    // AJUSTE 2: Recebemos o Tipo (1 = Venda, 2 = Baixa) direto do GadoFront
    public TipoBaixa Tipo { get; set; } 
    
    // O valor combinado com o frigorífico no dia
    public decimal PrecoArroba { get; set; } 
    
    // O valor total EXATO que o usuário fechou na tela (evita dízima e diferença de centavos)
    public decimal ValorTotal { get; set; }
    
    public List<ItemVendaDTO> Itens { get; set; } = new List<ItemVendaDTO>();
}

public class ItemVendaDTO
{
    public int AnimalId { get; set; }
    
    // O peso medido na fazenda antes de embarcar
    public decimal PesoVivo { get; set; }  
    
    // OPCIONAL: Se a venda for local e já tiver o peso da carcaça, pode mandar. 
    // Se for pro frigorífico, geralmente vai nulo e atualizamos depois.
    public decimal? PesoMorto { get; set; } 

    // --- SNAPSHOT (FOTOGRAFIA DO PASSADO) ---
    // O React agora vai enviar essas informações originais na hora da venda
    public string Raca { get; set; } 
    public decimal PesoEntrada { get; set; } 
    public decimal ValorEntrada { get; set; } 
}