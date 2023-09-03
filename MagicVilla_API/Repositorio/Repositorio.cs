using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class //El T es de generico 
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; //Convierte la variable T en una entidad del tipo DbSet

        public Repositorio(ApplicationDbContext db) //Constructor al que se le inyecta el applicationdbcontext 
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad); //Agrega el registro 
            await Grabar();
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync(); //Graba los cambios
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet; //Esto puede hacer consultas dentro del metodo 
            if (!tracked)  
            {
                query = query.AsNoTracking(); //Aqui se aplica el AsNoTracking si recibe el booleano de tracked como false 
            }
            if (filtro != null) 
            {
                query = query.Where(filtro); //Esto filtra el query con el filtro dado 
            }
            return await query.FirstOrDefaultAsync(); //Devuelve un registro 
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null) 
            {
                query = query.Where(filtro); //Aplica el filtro 
            }
            return await query.ToListAsync(); //Devuelve la lista 
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }
    }
}
