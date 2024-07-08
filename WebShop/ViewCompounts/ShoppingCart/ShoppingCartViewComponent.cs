using Microsoft.AspNetCore.Mvc;
using myShop.Entity.Repository;
using myShop.Uititlity;
using System.Security.Claims;
namespace myShop.Web.ViewCompount.ShoppingCart
 
{
    
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var user = clamidentity.FindFirst(ClaimTypes.NameIdentifier);

            if(user != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionKey)!= null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
                else
                {
                    var val = unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == user.Value).Count();
                    HttpContext.Session.SetInt32(SD.SessionKey, val);
                   return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
