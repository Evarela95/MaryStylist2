//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
