using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;

namespace MaterialDemo.ViewModels.Pages.Business.VObject
{
    public partial class StockIndexItem : ObservableObject
    {

        public long? ShelfId { get; set; }

        public long? MaterialId { get; set; }

        public long? TagId { get; set; }

        // 货位号
        public string? Code { get; set; }

        public string? BarCode { get; set; }

        public string? WarehouseName { get; set; }

        public string? ShelvesCode { get; set; }

        public string? ShelvesType { get; set; }

        public int? TakeSize { get; set; }

        [ObservableProperty]
        public int? quantity;

        public int? QuantityUpperLimit { get; set; }

        public int? QuantityLowerLimit { get; set; }

        public int? ScalesAddress { get; set; }

        public BaseStatusEnum ScalesStatus { get; set; }

        public string? ScalesModel { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string? CreateBy { get; set; }

        public string? UpdateBy { get; set; }
        public string? Remark { get; set; }

        public StockMaterial? StockMaterial { get; set; }

        public ElectronicTag? ElectronicTag { get; set; }


        private int cur_quantity { get; set; }

        private int difference { get; set; }
    }
}
