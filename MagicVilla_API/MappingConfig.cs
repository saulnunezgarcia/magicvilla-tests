using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;

namespace MagicVilla_API
{
    public class MappingConfig: Profile //La clase debe de heredar del paquete AutoMapper
    {
        public MappingConfig() //Aqui se detallan todos los mapeos en formato fuente, destino 
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap(); //El reverse map hace lo mismo que se hizo en las dos lineas anteriores pero en 
            // una sola linea 
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
        }  
    }
}
