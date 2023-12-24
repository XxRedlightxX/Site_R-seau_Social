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
            var utilisateurs = await _context.Utilisateurs.Include(u => u.Posts).ToListAsync();
            return View(utilisateurs);
        }

        public async Task<IActionResult> Profil()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);

                var userWithPosts = await _context.Utilisateurs
                    .Include(u => u.Posts)
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
                    return Json(new { success = false, error = "User not authenticated" });
                }

                var currentUser = await _userManager.GetUserAsync(User);

                if (userId == currentUser.Id)
                {
                    return Json(new { success = false, error = "Cannot add yourself as a friend" });
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
                    var updateResult = await _userManager.UpdateAsync(currentUser);

                    if (updateResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        return Json(new { success = false, error = $"Failed to update user: {errors}" });
                    }
                }
                else
                {
                    return Json(new { success = false, error = "User is already a friend" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}