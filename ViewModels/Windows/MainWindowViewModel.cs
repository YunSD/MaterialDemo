﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using HandyControl.Tools.Extension;
using System.Collections.ObjectModel;

namespace MaterialDemo.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = System.Configuration.ConfigurationManager.AppSettings["name"];

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            //new NavigationViewItem()
            //{
            //    Content = "Home",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            //    TargetPageType = typeof(Views.Pages.DashboardPage)
            //},
            //new NavigationViewItem()
            //{
            //    Content = "Data",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            //    TargetPageType = typeof(Views.Pages.DataPage)
            //}
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            //new NavigationViewItem()
            //{
            //    Content = "Settings",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            //    TargetPageType = typeof(Views.Pages.SettingsPage)
            //}
        };

        [ObservableProperty]
        private ObservableCollection<object> _trayMenuItems = new()
        {
            //new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
