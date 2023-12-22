document.addEventListener("DOMContentLoaded", function () {
    var chatFormContainer = document.getElementById('chat-form-container');
    var chatForm = document.getElementById('chat-form');
    var createRoomBtn = document.getElementById('create-room-btn');
    var chatMessagesContainer = document.getElementById('chat-messages-container');

    createRoomBtn.onclick = function () {
        toggleChatForm();
    };

    chatForm.addEventListener('submit', function (event) {
        event.preventDefault();
        var messageInput = document.getElementById('message-input');
        var message = messageInput.value;

        // Récupérer le nom d'utilisateur
        var username = chatFormContainer.dataset.username;

        // Ajouter le message à l'élément de la page avec le nom d'utilisateur
        appendMessage(username, message);

        // Effacer le champ de saisie
        messageInput.value = '';
    });

    function toggleChatForm() {
        if (chatFormContainer) {
            chatFormContainer.classList.toggle('active');
        } else {
            console.log('Element not found: chat-form-container');
        }
    }


    function appendMessage(username, message, isUserOnline) {
        // Créer un élément de paragraphe pour le message
        var messageElement = document.createElement('p');


        // Créer un élément pour l'icône utilisateur
        var userIcon = document.createElement('i');
        userIcon.classList.add('fa', 'fa-user');
        userIcon.style.marginRight = '5px'; // Ajoute une marge entre l'icône et le nom d'utilisateur

        // Ajouter l'icône utilisateur à l'élément de message
        messageElement.appendChild(userIcon);

        // Créer un élément pour l'icône de présence en ligne
        //  if (isUserOnline) {
        var onlineIcon = document.createElement('i');
        onlineIcon.classList.add('fa', 'fa-check-circle', 'text-success', 'online-icon');
        onlineIcon.style.fontSize = '0.7em'; // Ajuster la taille de l'icône
        onlineIcon.style.marginLeft = '5px'; // Ajouter une marge entre les icônes
        // Ajouter l'icône en ligb à l'élément de message
        messageElement.appendChild(onlineIcon);
        //  usernameElement.appendChild(onlineIcon);
        //    }

        // Ajouter le nom d'utilisateur au message
        var usernameElement = document.createElement('strong');
        usernameElement.style.marginRight = '5px';
        usernameElement.textContent = username;



        // Ajouter le nom d'utilisateur à l'élément de message
        messageElement.appendChild(usernameElement);

        // Créer un élément pour la date
        var dateElement = document.createElement('span');
        dateElement.classList.add('message-date');
        var now = new Date();
        var options = { weekday: 'long', hour: '2-digit', minute: '2-digit' };
        var dateTimeString = now.toLocaleDateString('fr-FR', options);

        // Ajouter la date et le message à l'élément de la page
        dateElement.textContent = dateTimeString;
        messageElement.appendChild(dateElement);
        messageElement.appendChild(document.createTextNode(' ' + message));

        // Ajouter le message à l'élément de la page
        chatMessagesContainer.appendChild(messageElement);
    }

});