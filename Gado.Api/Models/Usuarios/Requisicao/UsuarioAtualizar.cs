using System.ComponentModel.DataAnnotations;

namespace Gado.Api.Models.Requisicao;

public class UsuarioAtualizar
{
    [Required]
    public int ID { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public int TipoUsuarioId { get; set; }

    // 👇 Sem o [Required] e sem o "?"
    public string Senha { get; set; } 
}