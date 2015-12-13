using System.Security;

namespace TimesheetParser.Messages
{
    public class LoginMessage
    {
        public string Login { get; set; }
        public SecureString Password { get; set; }
    }
}