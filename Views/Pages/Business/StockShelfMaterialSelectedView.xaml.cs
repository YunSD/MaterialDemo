﻿using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfMaterialSelectedView : UserControl
    {
        public StockMaterialViewModel ViewModel { get; }

        private FormSubmitEventHandler<StockMaterial> SubmitEvent;

        public StockShelfMaterialSelectedView(StockMaterialViewModel viewModel, FormSubmitEventHandler<StockMaterial> SubmitEvent)
        {
            this.ViewModel = viewModel;
            this.ViewModel.OnNavigatedTo();
            this.SubmitEvent = SubmitEvent;
            DataContext = this;
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.RootDialog)) return;
            var selectedRow = DataGrid.SelectedItem as StockMaterial;
            if (selectedRow == null) selectedRow = new();
            SubmitEvent(selectedRow);
            DialogHost.Close(BaseConstant.RootDialog);
        }
    }
}
