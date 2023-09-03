using static MagicVilla_Utilidad.DS;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET; // Este es el valor inicial 

        public string Url { get; set; } //Se encarga de la URL 

        public object Datos { get; set; } // Se encarga de los datos 
    }
}


