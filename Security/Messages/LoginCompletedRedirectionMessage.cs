using MaterialDemo.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.Security.Messages
{
    public sealed class LoginCompletedRedirectionMessage
    {
        private static readonly LoginCompletedRedirectionMessage CUR_INSTANCE = new LoginCompletedRedirectionMessage();
        private LoginCompletedRedirectionMessage() { }
        public static LoginCompletedRedirectionMessage instance() { return CUR_INSTANCE; }
    }
}
