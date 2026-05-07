using DataAccess.Contexto;

namespace DataAccess.Repositorios;

public abstract class BaseRepositorio
{
    protected readonly GadoContexto _contexto;

    protected BaseRepositorio(GadoContexto contexto)
    {
        _contexto = contexto;
    }
}