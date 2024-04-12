using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Models.Entity;
using MaterialDemo.Security.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Security
{

    public partial class SecurityUser : ObservableRecipient, IRecipient<LoginCompletedMessage>, IRecipient<LogoutMessage>
    {

        public static readonly SecurityUser SECURITY_USER = new SecurityUser();
        public SecurityUser() {
            this.IsActive = true;
        }

        #region field
        [ObservableProperty]
        public volatile UserInfo? info = null;
        // is sign 是否登录
        public volatile bool is_Sign = false;
        #endregion


        private bool recycleStatement(SysUser sysUser) {
            lock (this)
            {
                if (is_Sign != false) return false;
                UserInfo info = new UserInfo(sysUser.UserId, sysUser.Username, sysUser.Name, sysUser.Avatar, sysUser.Phone, sysUser.Email);
                this.is_Sign = true;
                this.Info = info;
                return true;
            }
        }

        private void logout() {
            lock (this) { 
                if (is_Sign == false) return;
                this.is_Sign = false;
                this.Info = null ;
            }
        }


        public record UserInfo
        {
            public readonly long? UserId;
            public readonly string? UserName;
            public readonly string? Name;
            public readonly string? Avatar;
            public readonly string? phone;
            public readonly string? Email;

            public UserInfo(long? userId, string? username, string? name, string? avatar, string? phone, string? email)
            {
                UserId = userId;
                UserName = username;
                Name = name;
                Avatar = avatar;
                this.phone = phone;
                Email = email;
            }
        }

        public void Receive(LoginCompletedMessage message)
        {
            if (recycleStatement(message.SysUser)) { 
                // 通知跳转
               WeakReferenceMessenger.Default.Send<LoginCompletedRedirectionMessage>();
            };
        }

        public void Receive(LogoutMessage message)
        {
            this.logout();
        }
    }
}
