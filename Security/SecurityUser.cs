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


        public SecurityUser(SysUser sysUser) {
            UserInfo info = new UserInfo( sysUser.UserId, sysUser.Username, sysUser.Name, sysUser.Avatar, sysUser.Phone, sysUser.Email);
        }

        #region field
        [ObservableProperty]
        public volatile UserInfo? info = null;
        // is sign 是否登录
        [ObservableProperty]
        public volatile bool is_Sign = false;
        #endregion




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
            UserInfo info = new UserInfo(sysUser.UserId, sysUser.Username, sysUser.Name, sysUser.Avatar, sysUser.Phone, sysUser.Email);
            throw new NotImplementedException();
        }

        public void Receive(LogoutMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
