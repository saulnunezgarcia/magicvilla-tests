using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public VillaRepositorio(ApplicationDbContext db) : base(db) //El base es debido a que Irepositorio tambien tiene inyectado el 
            //ApplicationDbContext 
        {
            _db = db;
            this.dbSet = _db.Set<Villa>();
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad .FechaActualizacion = DateTime.Now;
            _db.Villas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
