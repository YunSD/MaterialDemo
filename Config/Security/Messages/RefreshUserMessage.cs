namespace MaterialDemo.Config.Security.Messages
{
    public sealed class RefreshUserMessage
    {
        public RefreshUserMessage(SecurityUser user) => User = user;

        public SecurityUser User { get; set; }
    }
}
