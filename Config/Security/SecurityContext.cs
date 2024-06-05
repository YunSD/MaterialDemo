using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Config.Security;
using MaterialDemo.Config.Security.Messages;

namespace MaterialDemo.Security
{

    public partial class SecurityContext : ObservableRecipient, IRecipient<LoginCompletedMessage>, IRecipient<LogoutMessage>
    {

        private static readonly SecurityContext SECURITY_USER = new SecurityContext();

        private SecurityContext() { this.IsActive = true; }

        public static SecurityContext Singleton { get { return SECURITY_USER; } }

        public SecurityUser? getUserInfo() {
            if (Sign) return SecurityUser;
            return null;
        }

       
        public static long? GetUserId() => Singleton.SecurityUser?.UserId;
        public static string? GetUserLoginName() => Singleton.SecurityUser == null ? "anonymous" : Singleton.SecurityUser.UserName;
        public static string? GetUserName() => Singleton.SecurityUser?.Name;



        #region field
        private volatile SecurityUser? SecurityUser;
        private volatile bool Sign = false;
        #endregion


        private bool recycleStatement(LoginCompletedMessage message) {
            lock (this)
            {
                if (Sign == true) return false;
                this.Sign = true;
                this.SecurityUser = message.User;
                return true;
            }
        }

        private void logout() {
            lock (this) { 
                if (Sign == false) return;
                this.Sign = false;
                this.SecurityUser = null ;
            }
        }

        public void Receive(LoginCompletedMessage message)
        {
            if(recycleStatement(message))  Messenger.Send<LoginCompletedRedirectionMessage>();
        }

        public void Receive(LogoutMessage message)
        {
            this.logout();
        }
    }
}
