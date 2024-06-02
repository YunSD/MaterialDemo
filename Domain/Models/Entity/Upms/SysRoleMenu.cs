using MaterialDemo.Config.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models.Entity.Upms
{

    [Table("sys_role_menu")]
    public class SysRoleMenu : BaseEntity
    {
        [Key]
        public long? Id { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; }

        [Column("menu_id")]
        public long? MenuId { get; set; }
    }
}
