// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Création d'une nouvelle instance de HubConnectionBuilder
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/learningHub") // Spécification de l'URL du hub SignalR
    .configureLogging(signalR.LogLevel.Information) // Configuration du niveau de
journalisation
    .build(); // Construction de l'objet HubConnection
// Définition d'une fonction de réception des messages du hub
connection.on("ReceiveMessage", (message) => {
    // Prépend le message reçu à un élément dans le DOM avec l'id 'signalr-messagepanel'
    $('#signalr-message-panel').prepend($('<div />').text(message));
});
// Gestionnaire d'événement pour le clic sur le bouton avec l'id 'btn-broadcast'
$('#btn-broadcast').click(function () {
    // Récupération du message à diffuser depuis un élément avec l'id 'broadcast'
    var message = $('#broadcast').val();
    // Invocation de la méthode côté serveur "BroadcastMessage" avec le message
    spécifié
    connection.invoke("BroadcastMessage", message).catch(err =>
        console.error(err.toString()));
});
// Fonction asynchrone pour démarrer la connexion
async function start() {
    try {
        // Tentative de démarrage de la connexion
        await connection.start();
        console.log('connected'); // Affichage dans la console si la connexion est
        établie
    } catch (err) {
        console.log(err); // Affichage dans la console en cas d'erreur de connexion
        setTimeout(() => start(), 5000); // Nouvelle tentative de connexion après 5
secondes en cas d'échec
    }
}
// Gestionnaire d'événement pour la fermeture de la connexion
connection.onclose(async () => {
    await start(); // Redémarrage de la connexion en cas de fermeture
});
// Démarrage initial de la connexion
start();