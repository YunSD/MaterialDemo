using MaterialDemo.Domain.Models.Entity;

namespace MaterialDemo.Config.Security.Messages
{
    public sealed class LoginCompletedMessage
    {
        public LoginCompletedMessage(SysUser user) => SysUser = user;


        public SysUser SysUser { get; set; }
    }
}
