using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Modelos
{
    public class Villa
    {
        [Key] //Asi se especifica que propiedad sera la clave primaria 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Esto permite tener un control del Id de cada elemento en la base de datos 
        public int Id { get; set; }
        public string Nombre { get; set; } //Se pueden agregar mas propiedades al modelo 
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
