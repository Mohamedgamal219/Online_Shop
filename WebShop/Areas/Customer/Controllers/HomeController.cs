using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.Entity.Models;
using myShop.Entity.Repository;
using myShop.Entity.ViewModels;
using myShop.Uititlity;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using X.PagedList;

namespace myShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index(int ? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 8;

            var all = _unitOfWork.Product.GetAll().ToPagedList(pageNumber , pageSize);
            return View(all);
        }
        [HttpGet]
        public IActionResult Details(int Id)
        {
            var obj = new ShoppingCart()
            { 
                Product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == Id, Includeword: "Category"),
                Count = 1,
                ProductId = Id
            };
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var user = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUsersId = user.Value;
            shoppingCart.Product = null;
            shoppingCart.Id = 0;

            ShoppingCart cartobj = _unitOfWork.ShoppingCart.GetFirstOrDefualt(
                u => u.ApplicationUsersId == user.Value && u.ProductId == shoppingCart.ProductId
                );
            if(cartobj == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Complete();

                var val = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == user.Value)
                    .ToList().Count();
                HttpContext.Session.SetInt32(SD.SessionKey, val);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(cartobj, shoppingCart.Count);
                _unitOfWork.Complete();

            }


            return RedirectToAction("Index");
        }
    }
}
