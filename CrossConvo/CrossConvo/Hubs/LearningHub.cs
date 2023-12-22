using CrossConvo.Models;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CrossConvo.Hubs
{

    [HubName("chatHub")]
    public class LearningHub : Hub<ILearningHubClient>
    {

        public async Task SendMessageToAll(string username, string message) { 
            await Clients.All.ReceiveMessage(username, message);
        }
        // Méthode pour diffuser un message à tous les clients connectés
        public async Task BroadcastMessage(string username, string message)
        {
            await Clients.All.ReceiveMessage(username, GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à tous les autres clients, sauf le destinataire actuel
        public async Task SendToOthers(string username, string message)
        {
            await Clients.Others.ReceiveMessage(username, GetMessageToSend(message));
        }

        // Méthode pour envoyer un message au client appelant (celui qui a initié l'appel)
        public async Task SendToCaller(string username, string message)
        {
            await Clients.Caller.ReceiveMessage(username, GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à un client individuel en utilisant son ID de connexion
        public async Task SendToIndividual(string username, string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessage(username, GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à tous les clients d'un groupe spécifique
        public async Task SendToGroup(string username, string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessage(username, GetMessageToSend(message));
        }

        // Méthode pour ajouter un utilisateur à un groupe spécifique
        public async Task AddUserToGroup(string username, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage(username, $"L'utilisateur actuel a été ajouté au groupe {groupName}");
            await Clients.Others.ReceiveMessage(username, $"L'utilisateur {Context.ConnectionId} a été ajouté au groupe {groupName}");
        }

        // Méthode pour retirer un utilisateur d'un groupe spécifique
        public async Task RemoveUserFromGroup(string username, string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage(username, $"L'utilisateur actuel a été retiré du groupe {groupName}");
            await Clients.Others.ReceiveMessage(username, $"L'utilisateur {Context.ConnectionId} a été retiré du groupe {groupName}");
        }

        // Méthode appelée lorsqu'un client se connecte
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Méthode appelée lorsqu'un client se déconnecte
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        // Méthode utilitaire pour formater un message avec l'ID de connexion du client
        private string GetMessageToSend(string originalMessage)
        {
            return $"ID de connexion de l'utilisateur : {Context.ConnectionId}. Message : {originalMessage}";
        }
    }
}
