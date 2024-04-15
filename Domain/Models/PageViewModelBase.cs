using MaterialDemo.Domain.Models.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain.Models
{
    public partial class PageViewModelBase<T> : ObservableObject
    {
        [ObservableProperty]
        public IList<T> dataList;

        // 每页显示多少条
        [ObservableProperty]
        public int pageSize = 20;

        // 总条目数
        [ObservableProperty]
        public int totalCount = 0;

        // 当前页
        [ObservableProperty]
        public int pageIndex = 0;

        // 页面总数
        [ObservableProperty]
        public int totalPage = 0;

        public PageViewModelBase()
        {
            dataList = new List<T>();
        }

    }
}
