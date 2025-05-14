using GreenHost.DAL;
using Microsoft.AspNetCore.Mvc;

namespace GreenHost.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var teamMembers = _context.TeamMembers.ToList();
            return View(teamMembers);
        }
    }
}
