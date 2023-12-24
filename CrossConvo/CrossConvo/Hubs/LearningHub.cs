using CrossConvo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace CrossConvo.Hubs
{
    public class LearningHub : Hub<ILearningHubClient>
    {
        private readonly UserManager<Utilisateur> _userManager;

        public LearningHub(UserManager<Utilisateur> userManager)
        {
            _userManager = userManager;
        }

        private async Task<Utilisateur> GetConnectedUser()
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            return user;
        }

        public async Task SendPrivateMessage(string friendId, string message)
        {
            var currentUser = await GetConnectedUser();
            var friend = await _userManager.FindByIdAsync(friendId);

            if (friend != null)
            {
                if (currentUser.Amis != null && currentUser.Amis.Any(a => a.UtilisateurId == friend.Id))
                {
                    await Clients.User(friend.Id).ReceivePrivateMessage(currentUser.Id, message);
                    await Clients.Caller.ReceivePrivateMessage(friend.Id, message); // Send the message to the sender as well
                }
            }
        }
    }
}