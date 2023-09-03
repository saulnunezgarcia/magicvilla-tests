using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Modelos

    // Aqui va los pasos para los modelos 

    //Creas el modelo mencionado en esta clase, despues creas sus respectivos Dto para general, update y create, luego se hacen las relaciones
    //de mapping en la clase mappingconfig y luego en DbContext se agrega el DbSet de NumeroVilla para que cuando se haga una migracion se 
    //cree una tabla nueva en la base de datos 
{
    public class NumeroVilla
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)] //No va a permitir repetir los valores pero permitira al usuario ingresar el 
        //numero 
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; } //Para relacionarla con la tabla villa 

        [ForeignKey("VillaId")] //Esto indica que se usara a VillaId como ForeignKey para la tabla villa 
        public Villa Villa { get; set; }

        public string DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        
    }
}
