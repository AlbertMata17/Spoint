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
    
    public partial class tiposproductos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tiposproductos()
        {
            this.productos = new HashSet<productos>();
        }
    
        public int idtipoproducto { get; set; }
        public string nombre { get; set; }
        public Nullable<int> empresaid { get; set; }
        public Nullable<int> usuarioid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productos> productos { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Login Login { get; set; }
    }
}
