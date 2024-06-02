using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.ViewModels.Pages.Business;
using Microsoft.EntityFrameworkCore;

namespace MaterialDemo.Config.EFDB
{
    public class BaseDbContext : DbContext
    {

        public DbSet<SysUser> Users { get; set; }
        public DbSet<SysMenu> Menus { get; set; }
        public DbSet<SysRole> Roles { get; set; }
        public DbSet<SysRoleMenu> roleMenus { get; set; }
        public DbSet<StockMaterial> StockMaterials { get; set; }
        public DbSet<StockMaterialStatement> StockMaterialStatements { get; set; }
        public DbSet<StockShelf> StockShelves { get; set; }
        public DbSet<ElectronicTag> ElectronicTags { get; set; }


        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>(e => {
                e.Property(e => e.LockFlag).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });

            modelBuilder.Entity<SysMenu>(e => {
                e.Property(e => e.Position).HasConversion(v => v.ToString(), v => (MenuPositionEnum)Enum.Parse(typeof(MenuPositionEnum), v));
            });


            modelBuilder.Entity<StockMaterialStatement>(e => {
                e.Property(e => e.Type).HasConversion(v => v.ToString(), v => (MaterialStatementTypeEnum)Enum.Parse(typeof(MaterialStatementTypeEnum), v));
            });

            modelBuilder.Entity<StockShelf>(e => {
                e.Property(e => e.ScalesStatus).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });

            modelBuilder.Entity<ElectronicTag>(e => {
                e.Property(e => e.ConnectStatus).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
                e.Property(e => e.WorkStatus).HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
