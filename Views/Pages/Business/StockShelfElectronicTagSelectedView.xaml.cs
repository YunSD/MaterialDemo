﻿using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;
using MaterialDemo.Utils;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfElectronicTagSelectedView : UserControl
    {
        public ElectronicTagViewModel ViewModel { get; }

        private FormSubmitEventHandler<ElectronicTag> SubmitEvent;

        public StockShelfElectronicTagSelectedView(ElectronicTagViewModel viewModel, FormSubmitEventHandler<ElectronicTag> SubmitEvent)
        {
            this.ViewModel = viewModel;
            this.ViewModel.OnNavigatedTo();
            this.SubmitEvent = SubmitEvent;
            DataContext = this;
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = DataGrid.SelectedItem as ElectronicTag;
            if (selectedRow == null) selectedRow = new ();
            SubmitEvent(selectedRow);
            DialogHost.Close(BaseConstant.RootDialog);
        }
    }
}
