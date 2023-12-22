using Google.Protobuf.Collections;
using Microsoft.AspNetCore.SignalR;

namespace CrossConvo.Hubs
{
    public class LearningHub : Hub<ILearningHubClient>
    {
        // Méthode pour diffuser un message à tous les clients connectés
        public async Task BroadcastMessage(string message)
        {
            await Clients.All.ReceiveMessage(GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à tous les autres clients, sauf le destinataire actuel
        public async Task SendToOthers(string message)
        {
            await Clients.Others.ReceiveMessage(GetMessageToSend(message));
        }

        // Méthode pour envoyer un message au client appelant (celui qui a initié l'appel)
        public async Task SendToCaller(string message)
        {
            await Clients.Caller.ReceiveMessage(GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à un client individuel en utilisant son ID de connexion
        public async Task SendToIndividual(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessage(GetMessageToSend(message));
        }

        // Méthode pour envoyer un message à tous les clients d'un groupe spécifique
        public async Task SendToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessage(GetMessageToSend(message));
        }

        // Méthode pour ajouter un utilisateur à un groupe spécifique
        public async Task AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"L'utilisateur actuel a été ajouté au groupe {groupName}");
            await Clients.Others.ReceiveMessage($"L'utilisateur {Context.ConnectionId} a été ajouté au groupe {groupName}");
        }

        // Méthode pour retirer un utilisateur d'un groupe spécifique
        public async Task RemoveUserFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"L'utilisateur actuel a été retiré du groupe {groupName}");
            await Clients.Others.ReceiveMessage($"L'utilisateur {Context.ConnectionId} a été retiré du groupe {groupName}");
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
