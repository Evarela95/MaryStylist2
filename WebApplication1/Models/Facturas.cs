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
    
    public partial class Facturas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Facturas()
        {
            this.Detalle_Factura_Productos = new HashSet<Detalle_Factura_Productos>();
            this.Detalle_Ganancias_Facturas = new HashSet<Detalle_Ganancias_Facturas>();
        }
    
        public int Id_Factura { get; set; }
        public Nullable<int> Id_Cita { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string User_Id { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Citas Citas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_Factura_Productos> Detalle_Factura_Productos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_Ganancias_Facturas> Detalle_Ganancias_Facturas { get; set; }
    }
}
