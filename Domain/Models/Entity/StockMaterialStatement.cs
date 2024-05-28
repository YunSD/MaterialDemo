using MaterialDemo.Config.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models.Entity
{
    [Table("stock_material_statement")]
    public class StockMaterialStatement : BaseEntity
    {

        [Key]
        [Column("statement_id")]
        public long? StatementId { get; set; }

        [Column("material_name")]
        public string? MaterialName { get; set; }

        [Column("material_code")]
        public string? MaterialCode { get; set; }

        [Column("material_model")]
        public string? MaterialModel { get; set; }

        [Column("material_unit")]
        public string? MaterialUnit { get; set; }

        [Column("shelf_info")]
        public string? ShelfInfo { get; set; }

        public MaterialStatementTypeEnum Type {  get; set; }


        [Column("before_stock")]
        public string? BeforeStock { get; set; }

        [Column("amount")]
        public string? Amount { get; set; }

        [Column("after_stock")]
        public string? AfterStock { get; set; }

        [Column("operator_id")]
        public long? OperatorId { get; set; }

        [Column("operator_name")]
        public string? OperatorName { get; set; }



    }

    public enum MaterialStatementTypeEnum{
    
        TAKE,
        SAVE

    }
}
