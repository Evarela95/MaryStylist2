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
    
    public partial class PromocionesServicios
    {
        public int Id_Promo_Servicio { get; set; }
        public string Nombre_Promo { get; set; }
        public string Descripcion_Promo { get; set; }
        public decimal Precio_Promo { get; set; }
        public System.DateTime Fecha_Inicio { get; set; }
        public System.DateTime Fecha_Final { get; set; }
        public int Id_Servicio { get; set; }
    
        public virtual Servicios Servicios { get; set; }
    }
}
