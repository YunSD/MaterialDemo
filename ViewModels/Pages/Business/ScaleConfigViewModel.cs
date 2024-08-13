using HI.Scale;
using Linq.PredicateBuilder;
using MaterialDemo.Controls;
using MaterialDemo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class ScaleConfigViewModel: ObservableObject, INavigationAware
    {
        /// <summary>
        /// 传感器状态
        /// </summary>
        [ObservableProperty]
        private bool _SensorConnectionStatus;

        /// <summary>
        /// 采集程序状态
        /// </summary>
        [ObservableProperty]
        private bool _DataAcquisitionProgramStatus;

        [ObservableProperty]
        private int _SlaveId;

        [ObservableProperty]
        private int _ZeroDemarcate;

        [ObservableProperty]
        private int _FullDemarcate;
        
        [ObservableProperty]
        private int _WeightDemarcate;

        [ObservableProperty]
        private int _Weight;

        [ObservableProperty]
        private bool _CountSwitch;

        [ObservableProperty]
        private int _MonomerMagnification;

        [ObservableProperty]
        private int _MonomerWeight;

        [ObservableProperty]
        private int _CurrentCount;

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo()
        {
            SensorConnectionStatus = DataAcquisitionService.Singleton.RequestScaleConnectStatus();
            DataAcquisitionProgramStatus = DataAcquisitionService.Singleton.RequestWorkeerStatus();
        }

        /// <summary>
        /// 加载所有数据
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestAllData() {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        private async Task RequestData() {
            IList<int> result = await DataAcquisitionService.Singleton.ScaleRequestAllData(SlaveId);
            ZeroDemarcate = result[0];
            FullDemarcate = result[1];
            WeightDemarcate = result[2];
            Weight = result[3];
            CountSwitch = result[4] == 1 ? true : false;
            MonomerMagnification = result[5];
            MonomerWeight = result[6];
            CurrentCount = result[7];
        }

        /// <summary>
        /// 零点标定
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteZeroDemarcate() {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteZeroDemarcate(SlaveId);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 满载标定
        /// </summary>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteFullDemarcate()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteFullDemarcate(SlaveId);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 砝码标定
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteWeightDemarcate()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteWeightDemarcate(SlaveId, WeightDemarcate);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 计数开关
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteCountSwitch()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteCountSwitch(SlaveId, CountSwitch ? 1 : 0);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 单体倍率
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="magnification"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteMonomerMagnification()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteMonomerMagnification(SlaveId, MonomerMagnification);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 单体重量
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteMonomerWeight()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteMonomerWeight(SlaveId, MonomerWeight);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }

        /// <summary>
        /// 当前数量
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task RequestWriteCurrentCount()
        {
            var waiting = new WaitingDialog("正在下发指令，请稍微...");
            _ = DialogHost.Show(waiting, BaseConstant.BaseDialog);
            await DataAcquisitionService.Singleton.RequestWriteCurrentCount(SlaveId, CurrentCount);
            await this.RequestData();
            DialogHost.Close(BaseConstant.BaseDialog);
        }
    }
}
