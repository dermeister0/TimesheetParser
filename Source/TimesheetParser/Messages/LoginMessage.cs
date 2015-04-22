using System.Security;

namespace TimesheetParser.Messages
{
    internal class LoginMessage
    {
        public string Login { get; set; }
        public SecureString Password { get; set; }
    }
}