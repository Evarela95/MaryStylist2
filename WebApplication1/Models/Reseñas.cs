//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reseñas
    {
        public int Id_Resena { get; set; }
        public string Comentario { get; set; }
        public Nullable<byte> Calificacion { get; set; }
        public string Id_Usuario { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
