namespace MaterialDemo.ViewModels.Pages
{
    using CommunityToolkit.Mvvm.Messaging;
    using MaterialDemo.Config.UnitOfWork;
    using MaterialDemo.Models.Entity;
    using MaterialDemo.Security.Messages;
    using MaterialDemo.Utils;
    using System;
    using System.Security;

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        /// <summary>
        /// Defines the _unitOfWork
        /// </summary>
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Defines the username
        /// </summary>
        [ObservableProperty]
        public string? username;

        /// <summary>
        /// Sets the password
        /// </summary>
        public SecureString? password { private get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWork"/></param>
        public LoginViewModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        //[RelayCommand]
        //public void LoginMethod() {
        //    if (this.DataContext != null)
        //    { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }

        //    var repository = _unitOfWork.GetRepository<SysUser>();
        //    List<SysUser> users = _unitOfWork.GetRepository<SysUser>().GetAll().ToList();
        //    Console.WriteLine("1");

        //}

        /// <summary>
        /// The SubmitByPassword
        /// </summary>
        /// <param name="param">The param<see cref="Object"/></param>
        [RelayCommand]
        public void SubmitByPassword(Object param)
        {
            PageUtil.ShowHostDialog(param);
            SysUser user = new SysUser();
            user.Username = "admin";
            user.Name = "admin";
            user.Avatar = "admin_avatar";
            user.Email = "admin email";
            user.Phone = "admin phone";
            WeakReferenceMessenger.Default.Send(new LoginCompletedMessage(user));
            PageUtil.CloseHostDialog();
        }
    }
}
