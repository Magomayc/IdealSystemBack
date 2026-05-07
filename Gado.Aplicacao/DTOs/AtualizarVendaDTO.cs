using System;
using System.Collections.Generic;

namespace Gado.Aplicacao.DTOs;

public class AtualizarVendaDTO
{
    public int VendaID { get; set; }
    public string Comprador { get; set; }
    public decimal PrecoArroba { get; set; }
    
    public List<ItemAtualizarDTO> Itens { get; set; } = new List<ItemAtualizarDTO>();
}

public class ItemAtualizarDTO
{
    public int AnimalId { get; set; }
    
    // O peso exato que vem na planilha do frigorífico 2 dias depois
    public decimal? PesoMorto { get; set; } 
}