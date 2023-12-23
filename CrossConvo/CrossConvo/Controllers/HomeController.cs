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
            var utilisateurs = await _context.Utilisateurs.ToListAsync();

            return utilisateurs.Any()
                ? View(utilisateurs)
                : Problem("Entity set 'ApplicationDbContext.Utilisateurs' is empty.");
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

        [HttpGet]
        public async Task<IActionResult> IncrementLikes(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post != null && _signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                post.IncrementLikes(currentUser.Id);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profil");
        }

        [HttpGet]
        public async Task<IActionResult> DecrementLikes(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post != null && _signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                post.DecrementLikes(currentUser.Id);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profil");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(Post post, IFormFile file)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Handle the case where the user is not authenticated, e.g., redirect to login
                return RedirectToAction("Login", "Account"); // Adjust this according to your authentication setup
            }

            // Retrieve the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Populate the UtilisateurId property with the current user's ID
            post.UtilisateurId = userId;

            // Set the publication date to the current date and time
            post.PublicationDate = DateTime.Now;

            // Handle file upload (if a file is provided)
            if (file != null && file.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    post.File = memoryStream.ToArray();
                }
            }

            // Add the post to your database context and save changes
            // (You need to have a DbContext configured and injected into the controller)
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Redirect to a success page or the home page
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddAmi(string friendId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(friendId))
            {
                var ami = new Ami
                {
                    UtilisateurId = currentUser.Id,
                };

                _context.Amis.Add(ami);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, errorMessage = "Invalid friendId" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}