﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Domain
{
    public class NavigationItem:ObservableObject
    {
        public string? Title { get; set; }
        public PackIconKind SelectedIcon { get; set; }
        public PackIconKind UnselectedIcon { get; set; }
        private object? _notification = null;

        public object? Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }
    }
}
