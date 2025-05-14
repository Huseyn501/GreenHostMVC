using GreenHost.DAL;
using GreenHost.Helpers.Extentions;
using GreenHost.Models;
using GreenHost.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenHost.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMembersController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment environment;

        public TeamMembersController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var teamMembers = _context.TeamMembers.ToList();
            return View(teamMembers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberVm teamMemberVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teamMemberVm.ImgFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImgFile", "File size must be less than 2MB");
                return View();
            }
            if (teamMemberVm.ImgFile.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("ImgFile", "File type must be image");
                return View();
            }
            
            TeamMembers teamMembers = new TeamMembers()
            {
                Name = teamMemberVm.Name,
                Position = teamMemberVm.Position,
                Imageurl = teamMemberVm.ImgFile.CreatingFile(environment.WebRootPath, "Upload")
            };
            await _context.TeamMembers.AddAsync(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            TeamMembers teamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if(teamMember == null)
            {
                return NotFound();
            }
            if(teamMember.Imageurl != null)
            {
                teamMember.Imageurl.DeletingFile(environment.WebRootPath, "Upload");
            }
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            TeamMembers teamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }
            TeamMemberVm teamMemberVm = new TeamMemberVm()
            {
                Name = teamMember.Name,
                Position = teamMember.Position,
            };
            return View(teamMemberVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,TeamMemberVm teamMemberVm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            TeamMembers teamMembers = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (teamMembers == null)
            {
                return NotFound();
            }
            if (teamMemberVm.ImgFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImgFile", "File size must be less than 2MB");
                return View();
            }
            if (teamMemberVm.ImgFile.ContentType.Contains("Image"))
            {
                ModelState.AddModelError("ImgFile", "File type must be image");
                return View();
            }
            if(teamMemberVm.ImgFile != null)
            {
                if(teamMembers.Imageurl != null)
                {
                    teamMemberVm.ImgFile.CreatingFile(environment.WebRootPath, "Upload");
                }
                teamMembers.Imageurl.DeletingFile(environment.WebRootPath, "Upload");
            }
            teamMembers.Name = teamMemberVm.Name;
            teamMembers.Position = teamMemberVm.Position;
            teamMembers.Imageurl = teamMemberVm.ImgFile.CreatingFile(environment.WebRootPath, "Upload");
            _context.TeamMembers.Update(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
