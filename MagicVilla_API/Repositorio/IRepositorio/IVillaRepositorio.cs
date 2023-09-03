using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa> //Hereda la interface IRepositorio que toma a Villa 
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
