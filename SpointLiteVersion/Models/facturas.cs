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
    
    public partial class facturas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public facturas()
        {
            this.DetalleVenta = new HashSet<DetalleVenta>();
        }
    
        public int idfactura { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public string observacion { get; set; }
        public string cliente { get; set; }
        public string vendedor { get; set; }
        public string producto { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> precio { get; set; }
        public string credito { get; set; }
        public byte[] Pdf { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
