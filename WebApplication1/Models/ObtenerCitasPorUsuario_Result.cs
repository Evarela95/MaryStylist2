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
    
    public partial class ObtenerCitasPorUsuario_Result
    {
        public int Id_Cita { get; set; }
        public System.DateTime Fecha_Cita { get; set; }
        public System.TimeSpan Hora_Cita { get; set; }
        public string Id_Usuario { get; set; }
        public int Id_Empleado { get; set; }
        public int Id_Serv_Prod { get; set; }
        public bool Estado { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public Nullable<decimal> Precio_Promo { get; set; }
        public string Descripcion { get; set; }
        public string Nombre_Empleado { get; set; }
        public string Apellido_Empleado { get; set; }
    }
}
