namespace CrossConvo.Hubs
{
    public interface ILearningHubClient
    {
        Task ReceiveMessage(string username, string message);
        Task ReceivePrivateMessage(string senderId, string message);
    }
}