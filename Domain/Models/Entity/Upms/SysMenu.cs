using MaterialDemo.Config.Db;
using MaterialDemo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MaterialDemo.Domain.Models.Entity
{
    [Table("sys_menu")]
    public class SysMenu : BaseEntity
    {

        [Key]
        [Column("menu_id")]
        public long? MenuId { get; set; }

        [Column("parent_id")]
        public long? ParentId { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Router { get; set; }
        public MenuPositionEnum Position { get; set; }
        public int? Seq { get; set; }

    }

    public enum MenuPositionEnum { 
        TOP,BOTTOM
    }
}
