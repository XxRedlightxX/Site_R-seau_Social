using CrossConvo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace CrossConvoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static Random rand = new Random();
        
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult Profil(int id)
        {

            //   Livre livre = SampleDonnes.Livres.FirstOrDefault(l => l.LivreId == id);
            Utilisateur utilisateur = _context.Utilisateurs.FirstOrDefault(l => l.UtilisateurId == id);
            return View(utilisateur);
        }





        [HttpGet]
        public ActionResult IncrementLikes()
        {
            Post post = new Post();
            // Récupérer la publication correspondant à postId depuis la base de données
            // et incrémenter le nombre de likes
            post.IncrementLikes();

            // Ensuite, retournez le nombre de likes mis à jour
            return RedirectToAction("ExplorerPage");
        }
    







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}