using MaterialDemo.Config.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialDemo.Domain.Models.Entity.Upms
{
    [Table("sys_role")]
    public class SysRole : BaseEntity
    {

        [Key]
        [Column("role_id")]
        public long? RoleId { get; set; }

        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
