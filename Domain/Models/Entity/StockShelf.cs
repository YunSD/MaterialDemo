﻿using MaterialDemo.Config.Db;
using MaterialDemo.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialDemo.Domain.Models.Entity
{

    [Table("stock_shelf")]
    public class StockShelf : BaseEntity
    {

        [Key]
        [Column("shelf_id")]
        public long? ShelfId { get; set; }

        [Column("material_id")]
        public long? MaterialId { get; set; }

        [Column("tag_id")]
        public long? TagId { get; set; }

        // 货位号
        public string? Code { get; set; }

        [Column("bar_code")]
        public string? BarCode { get; set; }

        [Column("warehouse_name")]
        public string? WarehouseName { get; set; }

        [Column("shelves_code")]
        public string? ShelvesCode { get; set; }

        [Column("shelves_type")]
        public string? ShelvesType { get; set; }

        [Column("take_size")]
        public int? TakeSize { get; set; }

        public int? Quantity { get; set; }

        [Column("quantity_upper_limit")]
        public int? QuantityUpperLimit { get; set; }

        [Column("quantity_lower_limit")]
        public int? QuantityLowerLimit { get; set; }

        [Column("scales_address")]
        public int? ScalesAddress { get; set; }

        [Column("scales_status")]
        public BaseStatusEnum ScalesStatus { get; set; }

        [Column("scales_model")]
        public string? ScalesModel { get; set; }
    }
}
