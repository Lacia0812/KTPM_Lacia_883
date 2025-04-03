namespace Lab1_THKTPM.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

