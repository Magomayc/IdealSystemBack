using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gado.Dominio.Enumeradores; // AJUSTE 1: Importamos o seu enumerador

namespace Gado.Api.Models.Requisicao;

public class VendaCriar
{
    // AJUSTE 2: Adicionamos o Tipo para a API saber logo de cara o que está recebendo
    public TipoBaixa Tipo { get; set; }

    // AJUSTE 3: Retiramos o [Required] porque em uma Baixa/Morte não existe comprador
    public string Comprador { get; set; }

    // Opcional: Se não passar, a aplicação define como "Agora"
    public DateTime DataVenda { get; set; }

    // AJUSTE 4: Adeus [Range(0.1, ...)] e [Required]! Agora o zero é muito bem-vindo aqui.
    public decimal PrecoArroba { get; set; }

    // Retiramos o [Required] para garantir que o zero passe liso
    public decimal ValorTotal { get; set; }

    [Required(ErrorMessage = "É necessário informar os animais que estão sendo vendidos/baixados")]
    [MinLength(1, ErrorMessage = "A operação deve conter pelo menos um animal.")]
    public List<ItemVendaCriar> Itens { get; set; } = new List<ItemVendaCriar>();
}

// O item que vai dentro da lista de criação (Neste não precisamos mexer, o PesoVivo ainda precisa ser maior que zero mesmo na morte)
public class ItemVendaCriar
{
    [Required(ErrorMessage = "É necessário informar o ID do Animal")]
    public int AnimalId { get; set; }

    [Required(ErrorMessage = "O peso vivo na balança da fazenda é obrigatório")]
    [Range(0.1, double.MaxValue, ErrorMessage = "O peso vivo deve ser maior que zero")]
    public decimal PesoVivo { get; set; }

    // Opcional no momento da saída do caminhão. Pode ser preenchido depois.
    public decimal? PesoMorto { get; set; }

    // --- A NOSSA FOTOGRAFIA ---
    public string Raca { get; set; }
    public decimal PesoEntrada { get; set; }
    public decimal ValorEntrada { get; set; }
}