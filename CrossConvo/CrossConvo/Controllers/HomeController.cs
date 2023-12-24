using CrossConvo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using NuGet.Packaging;

namespace CrossConvoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly UserManager<Utilisateur> _userManager;

        private static Random rand = new Random();

        public HomeController(ApplicationDbContext context, SignInManager<Utilisateur> signInManager, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public async Task<IActionResult> ExplorerPage()
        {
            var utilisateurs = await _context.Utilisateurs.Include(u => u.Posts).ThenInclude(p => p.Commentaires).ToListAsync();
            return View(utilisateurs);
        }

        public async Task<IActionResult> Profil()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var userWithPosts = await _context.Utilisateurs
                    .Include(u => u.Posts)
                    .Include(u => u.Amis)
                    .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

                return View(userWithPosts);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

       

        [HttpPost]
        public async Task<IActionResult> ToggleLike([FromForm] int postId, [FromForm] bool like)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await _context.Posts.FindAsync(postId);

            if (post != null)
            {
                var userHasLiked = post.LikedUserIds.Any(id => id == userId);
                if (like && !userHasLiked)
                {
                    post.Likes++;
                    post.LikedUserIds.Add(userId);
                }
                else if (!like && userHasLiked)
                {
                    post.Likes--;
                    post.LikedUserIds.Remove(userId);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ExplorerPage", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(Post post, IFormFile file)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            post.UtilisateurId = userId;

            post.PublicationDate = DateTime.Now;

            if (file != null && file.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    post.File = memoryStream.ToArray();
                }
            }
            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string userId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, error = "Authentifiez vous" });
                }

                var currentUser = await _userManager.GetUserAsync(User);

                if (userId == currentUser.Id)
                {
                    return Json(new { success = false, error = "Vous ne pouvez pas vous ajouter comme ami" });
                }

                var friendUser = await _userManager.FindByIdAsync(userId);

                if (friendUser == null)
                {
                    return Json(new { success = false, error = "User not found" });
                }

                if (currentUser.Amis == null)
                {
                    currentUser.Amis = new List<Ami>();
                }

                if (!currentUser.Amis.Any(a => a.UtilisateurId == userId))
                {
                    var newAmi = new Ami
                    {
                        Nom = friendUser.Nom,
                        Prenom = friendUser.Prenom,
                        Username = friendUser.UserName,
                        Email = friendUser.Email,
                        UtilisateurId = userId
                    };

                    currentUser.Amis.Add(newAmi);

                    var reverseAmi = new Ami
                    {
                        Nom = currentUser.Nom,
                        Prenom = currentUser.Prenom,
                        Username = currentUser.UserName,
                        Email = currentUser.Email,
                        UtilisateurId = currentUser.Id
                    };

                    friendUser.Amis ??= new List<Ami>();
                    friendUser.Amis.Add(reverseAmi);

                    var updateResultCurrentUser = await _userManager.UpdateAsync(currentUser);
                    var updateResultFriendUser = await _userManager.UpdateAsync(friendUser);

                    if (updateResultCurrentUser.Succeeded && updateResultFriendUser.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var errorsCurrentUser = string.Join(", ", updateResultCurrentUser.Errors.Select(e => e.Description));
                        var errorsFriendUser = string.Join(", ", updateResultFriendUser.Errors.Select(e => e.Description));
                        return Json(new { success = false, error = $"Failed to update users: {errorsCurrentUser}, {errorsFriendUser}" });
                    }
                }
                else
                {
                    return Json(new { success = false, error = "Cette utilisateur est déja votre ami" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string contenuCommentaire)
        {
            try
            {
                // Input validation
                if (postId <= 0 || string.IsNullOrWhiteSpace(contenuCommentaire))
                {
                    return RedirectToAction("ExplorerPage", "Home");
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var post = await _context.Posts.Include(p => p.Commentaires).FirstOrDefaultAsync(p => p.PostId == postId);

                if (post != null)
                {
                    var commentaire = new Commentaire
                    {
                        PublicationDate = DateTime.Now,
                        Contenu = contenuCommentaire.Trim(),
                        UtilisateurId = userId,
                        Likes = 0,
                        PostId = postId
                    };

                    post.Commentaires.Add(commentaire);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("ExplorerPage", "Home", new { postId = postId });
                }

                return RedirectToAction("ExplorerPage", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ExplorerPage", "Home");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Chat()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            await _context.Entry(currentUser).Collection(u => u.Amis).LoadAsync();

            return View(currentUser.Amis);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string friendId, string message)
        {
            if (string.IsNullOrEmpty(friendId) || string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}