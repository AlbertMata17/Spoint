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
    
    public partial class DetalleCompra
    {
        public int idDetalle { get; set; }
        public string codproducto { get; set; }
        public Nullable<int> idcompra { get; set; }
        public Nullable<int> cantidad { get; set; }
        public string descripcion { get; set; }
        public Nullable<decimal> costo { get; set; }
        public Nullable<decimal> importe { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }

        public DetalleCompra(string codproducto, int? idcompra, int? cantidad, string descripcion, decimal? costo, decimal? importe, DateTime? fecha, string tipo)
        {
            this.codproducto = codproducto;
            this.idcompra = idcompra;
            this.cantidad = cantidad;
            this.descripcion = descripcion;
            this.costo = costo;
            this.importe = importe;
            this.fecha = fecha;
            Tipo = tipo;
        }

        public DetalleCompra()
        {
        }

        public string Tipo { get; set; }
    }
}
