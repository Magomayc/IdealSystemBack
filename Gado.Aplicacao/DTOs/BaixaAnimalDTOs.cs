using System;
using System.Collections.Generic;

namespace Gado.Aplicacao.DTOs;

public class RegistrarBaixaDTO
{
    public int AnimalID { get; set; }
    public DateTime DataBaixa { get; set; }
    public string Motivo { get; set; }
    public string Observacao { get; set; }
}

public class AtualizarBaixaDTO
{
    public int BaixaID { get; set; }
    public DateTime DataBaixa { get; set; }
    public string Motivo { get; set; }
    public string Observacao { get; set; }
}