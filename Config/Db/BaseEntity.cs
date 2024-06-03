﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialDemo.Config.Db
{
    public class BaseEntity
    {
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }

        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }

        [Column("create_by")]
        public string? CreateBy { get; set; }

        [Column("update_by")]
        public string? UpdateBy {  get; set; }
        public string? Remark {  get; set; }

    }
}
