using MagicVilla_API.Modelos;
using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

// Este servicio sirve para tener una configuracion de todos los verbos GET,POST, etc

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        public APIResponse responseModel { get; set; }

        //Creamos constructor para inyectar los servicios 

        public IHttpClientFactory _httpClient {get; set;} //Esto sirve para cualquier tipo de conexiones 

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
                _httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI"); //Este el servicio 
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Message", "application/json"); //Se van a trabajar con requisitos Json
                message.RequestUri = new Uri(apiRequest.Url); //Obtiene la URL con la que se va a conectar 

                if (apiRequest.Datos != null) //diferente de null es un post o  put por lo que necesitas enviarle el contenido
                {
                    //Se convierte el mensaje a formato .Json 
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos), Encoding.UTF8, "applitcation/json");
                }

                switch (apiRequest.APITipo) //Se verifica el tipo de request con la que se va a trabajar 
                {
                    case DS.APITipo.POST:
                        message.Method = HttpMethod.Post; //Solo se define el metodo de HTTP con el que se va a trabajar 
                        break;
                    case DS.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get; 
                        break;
                }
                HttpResponseMessage apiResponse = null; 
                apiResponse = await client.SendAsync(message); //Se invoca el servicio, el cliente envia un mensaje y lo guarda en apiresponse
                var apiContent = await apiResponse.Content.ReadAsStringAsync(); //Lee el mensaje y lo guarda como string 
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); // Lo pasa a formato JSon 
                return APIResponse;


            }
            catch (Exception ex)
            {
                var dto = new APIResponse 
                {
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    IsExitoso = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
