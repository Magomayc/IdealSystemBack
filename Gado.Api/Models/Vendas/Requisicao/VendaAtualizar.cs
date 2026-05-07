using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores; // AJUSTE 1: Importamos o seu enumerador

namespace Gado.Api.Models.Requisicao;

public class VendaAtualizar
{
    [Required(ErrorMessage = "O ID da operação é obrigatório para a atualização")]
    public int ID { get; set; }

    // AJUSTE 2: Adicionamos o Tipo aqui também
    public TipoBaixa Tipo { get; set; }

    // AJUSTE 3: Tiramos o [Required] daqui, pois na Baixa o comprador pode ser vazio
    public string Comprador { get; set; }

    // AJUSTE 4: Tiramos o [Range(0.1, ...)] daqui! É ele quem gerava o erro 400 quando enviávamos zero.
    // A validação real agora acontece lá na camada de Aplicação, dependendo do Tipo.
    public decimal PrecoArroba { get; set; }

    public DateTime DataVenda { get; set; }

    [Required(ErrorMessage = "É necessário informar os itens")]
    public List<ItemVendaAtualizar> Itens { get; set; } = new List<ItemVendaAtualizar>();
}

// O item focado apenas em receber a atualização do frigorífico
public class ItemVendaAtualizar
{
    [Required(ErrorMessage = "O ID do animal é obrigatório")]
    public int AnimalId { get; set; }

    // O peso real da carcaça para o C# recalcular o valor e o rendimento
    // Usamos Range para garantir que, SE ele preencher, não seja um peso negativo
    [Range(0.1, double.MaxValue, ErrorMessage = "Se informado, o peso morto deve ser maior que zero")]
    public decimal? PesoMorto { get; set; } 
}