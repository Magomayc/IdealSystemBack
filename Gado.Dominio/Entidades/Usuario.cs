using Gado.Dominio.Enumeradores;

namespace Gado.Dominio.Entidades;

public class Usuario
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public TipoUsuarios TipoUsuarioID { get; set; }
    public bool Ativo { get; set; }

    public Usuario()
    {
        Ativo = true; 
    }

    public void Deletar()
    {
        Ativo = false;
    }

    public void Restaurar()
    {
        Ativo = true;
    }
}