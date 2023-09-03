using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{

    [Route("api/[controller]")] // Ruta de un controlador 
    [ApiController] //Indica que es un controlador 
    public class NumeroVillaController : ControllerBase //Esta clase hereda de controllerbase
    {
        //Se pueden añadir mensajes logger para la consola al momento en donde se este trabajando con los request Http

        private readonly ILogger<NumeroVillaController> _logger; //Los logger se deben de inicializar como variables privadas de esta forma 
        //private readonly ApplicationDbContext _db; Al crear la interface permite reemplazar el DBcontext por esta misma 
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, IMapper mapper,
             INumeroVillaRepositorio numeroRepo)
        {
            _logger = logger; //Este constructor es necesario para la ejecucion del logger
            _villaRepo = villaRepo;
            _mapper = mapper;
            _numeroRepo = numeroRepo;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Nuevo modelo sera una lista de villas 
        //Al escribir villa despues de IEnumerable, se especifica con ctrl+. que tome el directorio modelos
        //Al escrivir VillaDto se trabaja entonces con esa clase
        public async Task<ActionResult<ApiResponse>> GetNumeroVillas() //GetVillas es el nombre del metodo 
        {
            try
            {
                _logger.LogInformation("Obtener Numeros villas"); //El logger da un mensaje a la consola de cuando se estan obteniendo las villas

                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
            //Esto retorna un mapeo de los datos con IEnumerable, este recoge el 
            //formato VillaDto y toma los datos de villaList 

            // return Ok(await _db.Villas.ToListAsync()) // Esto trae desde la base de datos db la columna villas 


            //return Ok(VillaStore.villaList); //Esto regresa villalist de la clase Villastore
            //Esto es la primer parte antes de generar la clase VillaStore.cs que almacena los datos 
            //return new List<VillaDto> //Generara una lista basada en villa.cs
            //{
            //    new VillaDTO{ Id =1, Nombre = "Vista a la piscina"},
            //    new VillaDTO{ Id =2, Nombre = "Vista a la playa"}
            //};
        }

        // Y si solo quiero una sola villa?

        [HttpGet("id:int", Name = "GetNumeroVilla")] //el id:int es para dos cosas, diferenciar del anterior get y de especificar que es un dato de tipo int 
        [ProducesResponseType(StatusCodes.Status200OK)] // Para documentar los codigos de estado, con Status.code se pueden ver los
                                                        // otros tipos de codigos de estado, el nombre es para referenciar esta ruta 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ApiResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Numero Villa con Id" + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response); //Error del tipo 400
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroRepo.Obtener(x => x.VillaNo == id); //Esto es cuando ya se usa la base de datos db 

                if (numeroVilla == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

            //Usando el metodo FirstOrDefault, se le indica que por medio de v => v.Id == id,
            //se tomara el primer elemento v de villalist que tenga un atributo Id igual al de
            //la variable id, el => es igual a una funcion lambda en python 
        }

        [HttpPost] //Esto es para recibir datos
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto) //FromBody indica que vamos a recibir datos
        {
            try
            {
                if (!ModelState.IsValid) //ModelState te dice el estado del modelo y viene de Asp.NetCore, aqui se va a verificar si no es valido
                {
                    return BadRequest(ModelState); //De no ser valido, regresa un BadRequest
                }

                //Se puede dar una validacion personalizada 

                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    //El codigo de if checa si encontro un registro igual al registro que se trata de ingresar, si eso es diferente de null, 
                    //entonces se encontro un nombre y se va a arrogar este modelo de error 
                    ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe ");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(x => x.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de la villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto); //Si recibe nulo, entonces arroja un bad request
                }

                //if (villaDto.Id> 0) 
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError); //Si rec
                //}

                //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
                //VillaStore.villaList.Add(villaDto); Esto es cuando se quieren añadir datos sin una base de datos 

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto); //Esto crea un modelo en donde mapea los datos recibidos en CreateDto en una plantilla
                                                                          //del tipo Villa
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                //Villa modelo = new() 
                //{
                //    Nombre = villaDto.Nombre,
                //    Detalle = villaDto.Detalle,
                //    ImagenUrl = villaDto.ImagenUrl,
                //    Ocupantes = villaDto.Ocupantes,
                //    Tarifa = villaDto.Tarifa,
                //    MetrosCuadrados = villaDto.MetrosCuadrados,
                //    Amenidad = villaDto.Amenidad
                //}; //Este es un modelo para ingresar los datos, aqui no se usa la funcion de mapeado

                await _numeroRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, _response);
                //await _villaRepo.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;


            //Esto retorna la ruta a donde va a llegar el dato ingresado, se debe
            //de modifica el httpget para esto. Esto llama a la ruta "get villa", luego retorna el id que le estamos pasando y el ultimo 
            //parametro es porque requiere del villaDto enteramente
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id) ///Se usa IActionResult porque no trabaja con un modelo ya que siempre regresa un nocontent
        //IActionResult no acepta un tipo, por lo que no se puede especificar el ApiResponse entre <> como se hizo anteriormente, por lo tanto,
        //la forma de corregir esto es con un return BadRequest(_response); al final 
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response); //Si id es 0 devuelve BadRequest
                }
                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response); //Si el registro no esta en la lista, devuelve notfound
                }
                await _numeroRepo.Remover(numeroVilla); //Si coincide, lo remueve 

                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);

        }

        [HttpPut("{id:int}")] //Para actualizar un objeto 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto) //Recibe el id que le daremos pero tambien el registro que se va a actualizar 
        {
            if (updateDto == null || id != updateDto.VillaNo) // si el dato es nulo o si el id es distinto del villadto.id se devuelve un badrequest 
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            if (await _villaRepo.Obtener(x => x.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la Villa no existe");
                return BadRequest(ModelState);
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id); // Captura el registro antes de actualizarlo 

            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //}; //Este es un modelo para ingresar los datos 

            await _numeroRepo.Actualizar(modelo); //Si coincide, lo remueve 
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        // Para actualizar una sola propiedad por si el objeto tiene un monton de datos y si solo se busca actualizar uno en concreto 

    }
}
//Un endpoint es un metodo en c#
// Todos los endpoints deben de ir debajo de un verbo Http
// No se puede tener dos HttpGet sin diferenciar 

//Se necesita definir los tipos de retorno, cada endpoint tendra distintos codigos de estado y la forma
//de hacer esto es con ActionResult, esto se puede ver reflejado en el metodo id:int, si el usuario da un 
// int que no existe, se puede especificar el tipo de error, estos tipos de error necesitan ser documentados justo despues de los endpoints 

// Con la clase DBcontext se va a controlar que datos se van a usar como modelo para crear una tabla en la base de datos, 
// una vez creada la clase y haber especificado en el appsettings.json el DefaultConnection con el nombre del servidor, se escribe en la clase 
// program.cs el servicio que se usara (applicationdbcontext) y luego se escribe un constructor en el mismo applicationdbcontext. 
// finalmente, en el pm se escribe el comando Add-Migration AgregarBaseDatos para migrar y crear el formato de la tabla. Para que se ejecute 
// la base de datos se escribe en la consola update-database   

//Al crear los modelos siempre creamos objetos en otros, si las propiedades son demasiadas, se puede usar el Automapper  