﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.DTO
{   //Aqui solo se van a mostrar las propiedades que se van a querer mostrar cuando se expongan los datos 
    public class VillaUpdateDto
    {
        public int Id { get; set; }
        // Se pueden especificar propiedades necesarias para los datos, como el tipo de formato que deben de tener por ejemplo 
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; } //Se pueden agregar mas propiedades al modelo 
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        [Required]
        public int Ocupantes { get; set; }
        [Required]
        public int MetrosCuadrados { get; set; }
        [Required]
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }

    }
}
