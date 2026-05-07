using System.ComponentModel.DataAnnotations;

namespace Gado.Api.Models.Requisicao;

public class UsuarioLogin
{
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; }
}