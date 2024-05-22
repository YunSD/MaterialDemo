﻿using MaterialDemo.Config.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models.Entity
{
    [Table("stock_material")]
    public class StockMaterial : BaseEntity
    {

        [Key]
        [Column("material_id")]
        public long? MaterialId { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Model { get; set; }

        public string? Unit { get; set; }

        public string? Image { get; set; }

        [Column("max_quantity")]
        public int? MaxQuantity { get; set; }

        [Column("min_quantity")]
        public int? MinQuantity { get; set; }


    }
}
