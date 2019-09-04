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
    
    public partial class DetallePrefactura
    {
        public int IdDetalle { get; set; }
        public Nullable<int> idprefactura { get; set; }
        public Nullable<int> idventa { get; set; }
        public string Ref { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<decimal> precio { get; set; }
        public Nullable<decimal> descuento { get; set; }
        public string itbis { get; set; }
        public Nullable<decimal> importe { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totaldescuento { get; set; }
        public Nullable<int> idfactura { get; set; }
        public Nullable<int> empresaid { get; set; }
        public Nullable<int> usuarioid { get; set; }
        public Nullable<int> estatus { get; set; }

        public DetallePrefactura(int? idprefactura, int? idventa, string @ref, string descripcion, int? cantidad, decimal? precio, decimal? descuento, string itbis, decimal? importe, decimal? total, decimal? totaldescuento, int? idfactura, int? empresaid, int? usuarioid, int? estatus)
        {
            this.idprefactura = idprefactura;
            this.idventa = idventa;
            Ref = @ref;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
            this.precio = precio;
            this.descuento = descuento;
            this.itbis = itbis;
            this.importe = importe;
            this.total = total;
            this.totaldescuento = totaldescuento;
            this.idfactura = idfactura;
            this.empresaid = empresaid;
            this.usuarioid = usuarioid;
            this.estatus = estatus;
        }

        public DetallePrefactura()
        {
        }

        public virtual Empresa Empresa { get; set; }
        public virtual Login Login { get; set; }
        public virtual prefactura prefactura { get; set; }
    }
}