using CrossConvo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
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

            var applicationDbContext = _context.Posts.Include(p => p.Utilisateur);
            return View(await applicationDbContext.ToListAsync());

           
        }

        public async Task<IActionResult> Profil()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Get the currently signed-in user
                var currentUser = await _userManager.GetUserAsync(User);

                // Load the user's posts
                var userWithPosts = await _context.Utilisateurs
                    .Include(u => u.Posts)
                    .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

                return View(userWithPosts);
            }
            else
            {
                // Redirect to the login page or show an error message
                return RedirectToAction("Login", "Account");
            }
        }



        /*

        [HttpGet]
        public ActionResult IncrementLikes(int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post != null && _signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                post.IncrementLikes(currentUser.Id);
                _context.SaveChanges();
            }

            return RedirectToAction("Profil");
        }

        [HttpGet]
        public ActionResult DecrementLikes(int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post != null && _signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                post.DecrementLikes(currentUser.Id);
                _context.SaveChanges();
            }

            return RedirectToAction("Profil");
        }
        */


        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(Post post)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    post.PublicationDate = DateTime.Now;
                    post.Likes = 0;

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    post.UtilisateurId = userId;

                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Profil");
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it as needed
                ModelState.AddModelError(string.Empty, "An error occurred while saving the post.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            return View(post);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}