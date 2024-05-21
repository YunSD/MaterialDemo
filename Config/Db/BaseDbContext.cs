using log4net;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
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

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>(e =>
            {
                e.ToTable("sys_user");
                e.HasKey(e=>e.UserId);
                e.HasIndex(e => e.UserId);
                e.Property(e => e.LockFlag)
                .HasConversion(v => v.ToString(), v => (BaseStatusEnum)Enum.Parse(typeof(BaseStatusEnum), v));
            } 
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
