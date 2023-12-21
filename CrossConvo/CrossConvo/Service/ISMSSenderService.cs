namespace CrossConvo.Service
{
    public interface ISMSSenderService
    {
        Task SendSmsAsync(string number, string message);
    }
}
