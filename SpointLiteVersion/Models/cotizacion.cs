//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpointLiteVersion.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class cotizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cotizacion()
        {
            this.DetalleCotizacion = new HashSet<DetalleCotizacion>();
        }
    
        public int idcotizacion { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string observacion { get; set; }
        public string cliente { get; set; }
        public string vendedor { get; set; }
        public Nullable<decimal> precio { get; set; }
        public string credito { get; set; }
        public Nullable<int> idventa { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Status { get; set; }
        public Nullable<int> idcliente { get; set; }
        public Nullable<decimal> totalitbis { get; set; }
        public Nullable<int> empresaid { get; set; }
        public Nullable<int> usuarioid { get; set; }
        public Nullable<decimal> subtotal { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleCotizacion> DetalleCotizacion { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Login Login { get; set; }
    }
}