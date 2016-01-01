namespace TimesheetParser.Business.Services
{
    public interface IPasswordService
    {
        string Login { get; set; }
        string Password { get; set; }

        void LoadCredential();
        void SaveCredential();
        void DeleteCredential();
    }
}
