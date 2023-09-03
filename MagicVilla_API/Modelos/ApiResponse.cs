using System.Net;
using System.Runtime.InteropServices;

namespace MagicVilla_API.Modelos
{ //Esta clase se va a encargar de que todas las respuestas sean similares
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; } //Almacena el codigo de estado 
        public bool IsExitoso { get; set; } = true; 

        public List<string> ErrorMessages { get; set; } //Por si el endpoint tiene un error 

        public object Resultado { get; set; } // es objeto para permitir cualquier tipo de objeto
    }
}
