﻿using MaterialDemo.ViewModels.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Upms
{
    public partial class RoleViewPage : INavigableView<RoleViewModel>
    {
        public RoleViewModel ViewModel { get; }

        public RoleViewPage(RoleViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
