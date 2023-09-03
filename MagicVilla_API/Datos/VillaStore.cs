using MagicVilla_API.Modelos.DTO;
//Aqui se van a agregar datos ficticios en un store o almacenamiento 
namespace MagicVilla_API.Datos
{
    public static class VillaStore //se indica que es estatica porque los datos seran estaticos 
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto {Id = 1, Nombre = "Vista a la piscina", Ocupantes = 3, MetrosCuadrados = 50},
            new VillaDto {Id = 2, Nombre = "Vista a la playa", Ocupantes = 4, MetrosCuadrados = 80}
        };
    }
}
