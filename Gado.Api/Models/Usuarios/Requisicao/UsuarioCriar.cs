using System.ComponentModel.DataAnnotations;

namespace Gado.Api.Models.Requisicao;

public class UsuarioCriar
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; }

    [Required]
    public int TipoUsuarioId { get; set; }
}