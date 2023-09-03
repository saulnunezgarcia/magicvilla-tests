using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.DTO
{
    public class NumeroVillaDto
    {
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; } //Para relacionarla con la tabla villa 
        public string DetalleEspecial { get; set; }
    }
}
