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
    
    public partial class Facturas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Facturas()
        {
            this.Detalle_Ganancias_Facturas = new HashSet<Detalle_Ganancias_Facturas>();
            this.DetalleFacturaProducto = new HashSet<DetalleFacturaProducto>();
            this.DetalleFacturaServicios = new HashSet<DetalleFacturaServicios>();
        }
    
        public int Id_Factura { get; set; }
        public int Id_Cita { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Vuelto { get; set; }
    
        public virtual Citas Citas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_Ganancias_Facturas> Detalle_Ganancias_Facturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFacturaProducto> DetalleFacturaProducto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFacturaServicios> DetalleFacturaServicios { get; set; }
    }
}
