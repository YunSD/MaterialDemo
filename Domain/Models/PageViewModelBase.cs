using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models
{
    public partial class PageViewModelBase<T> : ObservableObject
    {
        [ObservableProperty]
        public IList<T>? dataList;

        // 每页显示多少条
        public int? pageSize;

        // 总条目数
        public int? totalCount;
        
        // 当前页
        public int? pageIndex;

        // 页面总数
        public int? totalPage;


    }
}
