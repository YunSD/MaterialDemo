using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.ViewModels.Pages.Business.VObject
{
    public class StockShelfViewInfo : StockShelf
    {
        public StockMaterial? StockMaterial { get; set; }

        public ElectronicTag? ElectronicTag { get; set; }
    }
}
