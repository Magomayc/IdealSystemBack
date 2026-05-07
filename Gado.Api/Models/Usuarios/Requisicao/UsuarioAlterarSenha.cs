using System.ComponentModel.DataAnnotations;

namespace Gado.Api.Models.Requisicao;

public class UsuarioAlterarSenha
{
    [Required]
    public int ID { get; set; }

    [Required(ErrorMessage = "A senha antiga é obrigatória")]
    public string SenhaAntiga { get; set; }

    [Required(ErrorMessage = "A nova senha é obrigatória")]
    public string Senha { get; set; }
}