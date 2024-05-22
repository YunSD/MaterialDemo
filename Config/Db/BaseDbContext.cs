using log4net;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Views.Pages.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Config.EFDB
{
    public class BaseDbContext : DbContext
    {

        public DbSet<SysUser> Users { get; set; }

        public DbSet<StockMaterial> StockMaterials { get; set; }
        public DbSet<StockMaterialStatement> StockMaterialStatements { get; set; }
        public DbSet<StockShelf> StockShelves { get; set; }
        public DbSet<ElectronicTag> ElectronicTags { get; set; }


        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>(e =>
            {
                e.Property(e => e.LockFlag).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });


            modelBuilder.Entity<StockMaterialStatement>(e => {
                e.Property(e => e.Type).HasConversion(v => v.ToString(), v => (MaterialStatementTypeEnum)Enum.Parse(typeof(MaterialStatementTypeEnum), v));
            });


            modelBuilder.Entity<ElectronicTag>(e => {
                e.Property(e => e.ConnectStatus).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
                e.Property(e => e.WorkStatus).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
