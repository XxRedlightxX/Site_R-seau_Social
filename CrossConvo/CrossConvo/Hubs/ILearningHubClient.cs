namespace CrossConvo.Hubs
{
    public interface ILearningHubClient
    {
        Task ReceiveMessage(string message);
    }
}
