// Crée une nouvelle connexion SignalR en utilisant la configuration spécifiée
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/learningHub") // Configure l'URL du hub
    .configureLogging(signalR.LogLevel.Information) // Configure le niveau de journalisation
    .build();

// Définit une méthode pour traiter les messages reçus du hub
connection.on("ReceiveMessage", (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-broadcast'
$('#btn-broadcast').click(function () {
    var message = $('#broadcast').val();
    // Invoque la méthode côté serveur 'BroadcastMessage' avec le message spécifié
    connection.invoke("BroadcastMessage", message).catch(err => console.error(err.toString()));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-others-message'
$('#btn-others-message').click(function () {
    var message = $('#others-message').val();
    // Invoque la méthode côté serveur 'SendToOthers' avec le message spécifié
    connection.invoke("SendToOthers", message).catch(err => console.error(err.toString()));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-self-message'
$('#btn-self-message').click(function () {
    var message = $('#self-message').val();
    // Invoque la méthode côté serveur 'SendToCaller' avec le message spécifié
    connection.invoke("SendToCaller", message).catch(err => console.error(err.toString()));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-group-message'
$('#btn-group-message').click(function () {
    var message = $('#group-message').val();
    var group = $('#group-for-message').val();
    // Invoque la méthode côté serveur 'SendToGroup' avec le groupe et le message spécifiés
    connection.invoke("SendToGroup", group, message).catch(err => console.error(err.toString()));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-group-add'
$('#btn-group-add').click(function () {
    var group = $('#group-to-add').val();
    // Invoque la méthode côté serveur 'AddUserToGroup' avec le groupe spécifié
    connection.invoke("AddUserToGroup", group).catch(err => console.error(err.toString()));
});

// Associe une fonction de rappel au clic du bouton avec l'ID 'btn-group-remove'
$('#btn-group-remove').click(function () {
    var group = $('#group-to-remove').val();
    // Invoque la méthode côté serveur 'RemoveUserFromGroup' avec le groupe spécifié
    connection.invoke("RemoveUserFromGroup", group).catch(err => console.error(err.toString()));
});

// Fonction asynchrone pour démarrer la connexion SignalR
async function start() {
    try {
        await connection.start(); // Tente de démarrer la connexion
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000); // En cas d'échec, réessaie après 5 secondes
    }
}

// Gère l'événement de fermeture de la connexion en redémarrant la connexion
connection.onclose(async () => {
    await start();
});

// Démarre la connexion lors du chargement de la page
start();
