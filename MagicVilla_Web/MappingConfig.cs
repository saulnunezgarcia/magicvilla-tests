using AutoMapper;
using MagicVilla_Web.Models.DTO;
using System.Runtime;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVillaDto, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVillaDto, NumeroVillaUpdateDto>().ReverseMap();
        }  
    }
}
