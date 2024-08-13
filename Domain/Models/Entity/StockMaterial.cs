using MaterialDemo.Config.Db;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? Weight { get; set; }

        public int? Magnification { get; set; }

        [Column("max_quantity")]
        public int? MaxQuantity { get; set; }

        [Column("min_quantity")]
        public int? MinQuantity { get; set; }

        [Column("dynamic_calibration")]
        public MaterialDynamicCalibrationEnum DynamicCalibration { get; set; }
    }
    
    public enum MaterialDynamicCalibrationEnum
    {
        OPEN,
        CLOSE
    }
}
