﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class spointEntities : DbContext
    {
        public spointEntities()
            : base("name=spointEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ciudad> ciudad { get; set; }
        public virtual DbSet<clientes> clientes { get; set; }
        public virtual DbSet<tiposproductos> tiposproductos { get; set; }
        public virtual DbSet<suplidores> suplidores { get; set; }
        public virtual DbSet<vendedores> vendedores { get; set; }
        public virtual DbSet<productos> productos { get; set; }
        public virtual DbSet<facturas> facturas { get; set; }
        public virtual DbSet<itbis> itbis { get; set; }
    }
}