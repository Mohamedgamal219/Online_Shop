using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entity.Repository;
using myShop.Uititlity;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    [Authorize (Roles = SD.AdminRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork )
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.order = unitOfWork.OrderHeader.GetAll().Count();
            ViewBag.category = unitOfWork.Category.GetAll().Count();
            ViewBag.user = unitOfWork.ApplicationUsers.GetAll().Count();
            ViewBag.product = unitOfWork.Product.GetAll().Count();
            return View();
        }
    }
}
