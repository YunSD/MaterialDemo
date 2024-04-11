using MaterialDemo.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Security.Messages
{
    public sealed class LoginCompletedMessage
    {
        public LoginCompletedMessage(SysUser user) => (SysUser) = (user);


        public SysUser SysUser { get; set; }
    }
}
