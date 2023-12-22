using Microsoft.AspNetCore.SignalR.Client;

// Demande à l'utilisateur de spécifier l'URL du SignalR Hub
Console.WriteLine("Veuillez spécifier l'URL du SignalR Hub");
var url = Console.ReadLine();

// Crée une connexion au hub SignalR en utilisant l'URL spécifiée
var hubConnection = new HubConnectionBuilder()
                         .WithUrl(url)
                         .Build();

// Configure une méthode pour traiter les messages reçus du hub
hubConnection.On<string>("ReceiveMessage",
    message => Console.WriteLine($"Message du SignalR Hub : {message}"));

try
{
    // Tente de démarrer la connexion au hub SignalR
    await hubConnection.StartAsync();

    var running = true;

    while (running)
    {
        var message = string.Empty;

        var groupName = string.Empty;

        // Affiche les options d'action disponibles
        Console.WriteLine("Veuillez spécifier l'action :");
        Console.WriteLine("0 - diffuser à tous");
        Console.WriteLine("1 - envoyer aux autres");
        Console.WriteLine("2 - envoyer à soi-même");
        Console.WriteLine("3 - envoyer à un individu");
        Console.WriteLine("4 - envoyer à un groupe");
        Console.WriteLine("5 - ajouter un utilisateur à un groupe");
        Console.WriteLine("6 - supprimer un utilisateur d'un groupe");
        Console.WriteLine("exit - Quitter le programme");

        // Lit l'action spécifiée par l'utilisateur
        var action = Console.ReadLine();

        // Si l'action n'est pas liée à l'ajout ou à la suppression d'un utilisateur d'un groupe, demande le message
        if (action != "5" && action != "6")
        {
            Console.WriteLine("Veuillez spécifier le message :");
            message = Console.ReadLine();
        }

        // Si l'action est liée à l'envoi à un groupe, à l'ajout ou à la suppression d'un utilisateur d'un groupe, demande le nom du groupe
        if (action == "4" || action == "5" || action == "6")
        {
            Console.WriteLine("Veuillez spécifier le nom du groupe :");
            groupName = Console.ReadLine();
        }

        // Exécute l'action spécifiée
        switch (action)
        {
            case "0":
                // Diffuse le message à tous les utilisateurs
                await hubConnection.SendAsync("BroadcastMessage", message);
                break;
            case "1":
                // Envoie le message aux autres utilisateurs
                await hubConnection.SendAsync("SendToOthers", message);
                break;
            case "2":
                // Envoie le message à soi-même
                await hubConnection.SendAsync("SendToCaller", message);
                break;
            case "3":
                // Demande l'ID de connexion et envoie le message à un utilisateur individuel
                Console.WriteLine("Veuillez spécifier l'ID de connexion :");
                var connectionId = Console.ReadLine();
                await hubConnection.SendAsync("SendToIndividual", connectionId, message);
                break;
            case "4":
                // Envoie le message à un groupe spécifié
                hubConnection.SendAsync("SendToGroup", groupName, message).Wait();
                break;
            case "5":
                // Ajoute l'utilisateur à un groupe spécifié
                hubConnection.SendAsync("AddUserToGroup", groupName).Wait();
                break;
            case "6":
                // Supprime l'utilisateur d'un groupe spécifié
                hubConnection.SendAsync("RemoveUserFromGroup", groupName).Wait();
                break;
            case "exit":
                // Quitte le programme
                running = false;
                break;
            default:
                Console.WriteLine("Action non valide spécifiée");
                break;
        }
    }
}
catch (Exception ex)
{
    // En cas d'erreur, affiche le message d'erreur et attend une touche pour quitter
    Console.WriteLine(ex.Message);
    Console.WriteLine("Appuyez sur n'importe quelle touche pour quitter...");
    Console.ReadKey();
    return;
}
