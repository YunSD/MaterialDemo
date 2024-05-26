using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;

namespace MaterialDemo.ViewModels.Pages.Business.VObject
{
    public class StockShelfViewInfo : StockShelf
    {
        public StockMaterial? StockMaterial { get; set; }

        public ElectronicTag? ElectronicTag { get; set; }
    }
}
