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
    
    public partial class suplidores
    {
        public int idSuplidor { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string correo { get; set; }
        public string cedula { get; set; }
        public string Foto { get; set; }
        public string Status { get; set; }
        public Nullable<int> TipoSuplidor { get; set; }
        public Nullable<int> LimiteTiempo { get; set; }
        public string Personafisica { get; set; }
        public Nullable<int> empresaid { get; set; }
        public Nullable<int> usuarioid { get; set; }
    
        public virtual TipoSuplidor TipoSuplidor1 { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Login Login { get; set; }
    }
}
