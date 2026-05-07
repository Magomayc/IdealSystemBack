using System;
using System.ComponentModel.DataAnnotations;

namespace Gado.Api.Models.Requisicao;

public class BaixaAnimalCriar
{
    [Required(ErrorMessage = "O ID do animal é obrigatório.")]
    public int AnimalID { get; set; }

    [Required(ErrorMessage = "A data da baixa é obrigatória.")]
    public DateTime DataBaixa { get; set; }

    [Required(ErrorMessage = "O motivo da baixa é obrigatório.")]
    [StringLength(100, ErrorMessage = "O motivo deve ter no máximo 100 caracteres.")]
    public string Motivo { get; set; }

    [StringLength(500, ErrorMessage = "A observação deve ter no máximo 500 caracteres.")]
    public string Observacao { get; set; }
}