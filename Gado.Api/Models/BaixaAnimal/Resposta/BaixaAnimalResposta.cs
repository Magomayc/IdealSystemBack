using System;

namespace Gado.Api.Models.Resposta;

public class BaixaAnimalResposta
{
    public int Id { get; set; }
    public DateTime DataBaixa { get; set; }
    public string Motivo { get; set; }
    public string Observacao { get; set; }
    public bool Ativo { get; set; }
    
    // Devolvemos o ID e a caixinha completa do animal para a tela
    public int AnimalID { get; set; }
    public AnimalResposta Animal { get; set; }
}