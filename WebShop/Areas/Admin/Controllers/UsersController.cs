using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Uititlity;
using System.Security.Claims;


namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;
            
            return View(_context.ApplicationUsers.Where(x=>x.Id != userid));
        }
        public IActionResult LockUnLock(string ? id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            if(user.LockoutEnd == null || user.LockoutEnd < DateTime.Now) 
            {
                user.LockoutEnd = DateTime.Now.AddDays(4);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }
    }
}
