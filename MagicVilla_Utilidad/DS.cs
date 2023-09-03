namespace MagicVilla_Utilidad
{
    public static class DS
    {
        public enum APITipo
        {   //Estas variables manejan diferentes tipos de valores segun el tipo de situacion
            GET,
            POST,
            PUT,
            DELETE,
        }
    }
}

//Hay que añadir a MagicVilla_Web la dependendia para Utilidad para que _Web pueda añadir 