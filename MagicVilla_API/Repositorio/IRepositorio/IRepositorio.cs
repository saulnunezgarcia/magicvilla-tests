using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{   //Aqui se creara un nuevo repositorio que se encargara del manejo de los datos en vez de administrarlos desde el controlador 
    public interface IRepositorio<T> where T : class //Esto hace que la interfaz sea generica, con T se puede recibir cualquier entidad 
    {
        Task Crear(T entidad); //Este metodo recibe la entidad 

        Task<List<T>> ObtenerTodos(Expression<Func<T,bool>>? filtro = null); //Va a devolver una lista segun la entidad que se le envie, tiene
        //un filtro denominado por Expression que es nulo en caso de que sea nulo, si la lista no es nula, se filtrara segun la condicion dada

        Task<T> Obtener(Expression<Func<T, bool>>? filtro = null,bool tracked = true); //Esto corrige el error tracking 

        Task Remover(T entidad); //Para remover una entidad dada 

        Task Grabar(); //Para salvar los cambios 
    }
}
