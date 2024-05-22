using MaterialDemo.Config.Db;
using MaterialDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models.Entity.Upms
{

    [Table("electronic_tag")]
    public class ElectronicTag :BaseEntity
    {
        [Key]
        [Column("tag_id")]
        public long? TagId { get; set; }


        public string? Code { get; set; }

        public string? Ip { get; set; }

        [Column("connect_status")]
        public BaseStatusEnum ConnectStatus { get; set; }

        [Column("work_status")]
        public BaseStatusEnum WorkStatus { get; set; }

    }
}
