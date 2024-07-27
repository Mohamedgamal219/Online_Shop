using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myShop.DataAccess.Implntation;
using myShop.Entity.DTO;
using myShop.Entity.Models;
using myShop.Entity.Repository;
using myShop.Entity.ViewModels;
using myShop.Uititlity;
using Stripe.Checkout;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;

namespace myShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        //public IActionResult Index()
        //{
        //    var claimidentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

        //    ShoppingCartVM = new ShoppingCartVM
        //    {
        //        CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product")
        //    };

        //    foreach (var item in ShoppingCartVM.CartList)
        //    {
        //        ShoppingCartVM.Total += (item.Count * item.Product.Price);
        //    }

        //    return View(ShoppingCartVM);
        //}

        public IActionResult Index()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);


            IEnumerable<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product");

            List<DTOShoppingCart> DtoShoppingCarts = new();

            foreach (var item in CartList) 
            {
                var DtoShoopingCart = new DTOShoppingCart
                {
                    Id = item.Id,
                    Product = item.Product,
                    ApplicationUsersId = item.ApplicationUsersId,
                    ProductId = item.ProductId,
                    Count = item.Count,
                };

                DtoShoppingCarts.Add(DtoShoopingCart);
                  
            }
            DTOShoppingCartSamary dTOShoppingCartSamary = new DTOShoppingCartSamary
            {
                dTOShoppingCarts = DtoShoppingCarts
            };


            foreach (var item in dTOShoppingCartSamary.dTOShoppingCarts)
            {
                dTOShoppingCartSamary.Total += (item.Count * item.Product.Price);
            };

            return View(dTOShoppingCartSamary);
        }

        public IActionResult Plus(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(x => x.Id == cartid);

            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
           
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public IActionResult Miuns(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(x => x.Id == cartid);

            if (shoppingcart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
                var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == shoppingcart.ApplicationUsersId).Count()-1;
                HttpContext.Session.SetInt32(SD.SessionKey, count);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
            }

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstOrDefualt(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.Remove(shoppingcart);
            _unitOfWork.Complete();

            var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == shoppingcart.ApplicationUsersId).Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction("Index");

        }
        //[HttpGet]
        //public IActionResult Sammary()
        //{
        //    var claimidentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

        //    var ShoppingCartVM = new ShoppingCartVM
        //    {
        //        CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product"),
        //        OrderHeader = new OrderHeader()
        //    };

        //    ShoppingCartVM.OrderHeader.ApplicationUsers = _unitOfWork.ApplicationUsers.GetFirstOrDefualt(x => x.Id == claim.Value);
        //    ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUsers.Name;
        //    ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUsers.City;
        //    ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUsers.Address;
        //    ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUsers.PhoneNumber;

        //    foreach (var item in ShoppingCartVM.CartList)
        //    {
        //        ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
        //    }

        //    return View(ShoppingCartVM);
        //}



        [HttpGet]
        public IActionResult Sammary()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product");

            List<DTOShoppingCart> DtoShoppingCarts = new();

            foreach (var item in CartList)
            {
                var DtoShoopingCart = new DTOShoppingCart
                {
                    Id = item.Id,
                    Product = item.Product,
                    ApplicationUsersId = item.ApplicationUsersId,
                    ProductId = item.ProductId,
                    Count = item.Count,
                };

                DtoShoppingCarts.Add(DtoShoopingCart);
            }
            

            ApplicationUsers application = _unitOfWork.ApplicationUsers.GetFirstOrDefualt(x => x.Id == claim.Value);

            DTOOrderHeader dTOOrderHeader = new DTOOrderHeader
            {
                ApplicationUsers = application,
                Name = application.Name,
                Address = application.Address,
                PhoneNumber = application.PhoneNumber,
                City = application.City
            };

            DTOShoppingCartSamary dTOShoppingCartSamary = new DTOShoppingCartSamary
            {
                dTOShoppingCarts = DtoShoppingCarts,
                DTOOrderHeader = dTOOrderHeader
                
                
            };
            
            foreach (var item in dTOShoppingCartSamary.dTOShoppingCarts)
            {
                dTOShoppingCartSamary.Total += (item.Count * item.Product.Price);
            };

            dTOShoppingCartSamary.DTOOrderHeader.TotalPrice = dTOShoppingCartSamary.Total;
            return View(dTOShoppingCartSamary);
        }

        //[HttpPost]
        //[ActionName("Sammary")]
        //[ValidateAntiForgeryToken]
        //public IActionResult ComfirmSammary( ShoppingCartVM ShoppingCartVM)
        //{
        //    var claimidentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

        //    ShoppingCartVM.CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product");
        //    ShoppingCartVM.OrderHeader.ApplicationUsersId = claim.Value;

        //    ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
        //    ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
        //    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

        //    foreach (var item in ShoppingCartVM.CartList)
        //    {
        //        ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
        //    }

        //    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        //    _unitOfWork.Complete();

        //    foreach (var item in ShoppingCartVM.CartList)
        //    {
        //        OrderDetails orderDetails = new OrderDetails
        //        {
        //            ProductId = item.ProductId,
        //            OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
        //            Price = item.Product.Price,
        //            Count = item.Count
        //        };
        //        _unitOfWork.OrderDetails.Add(orderDetails);
        //        _unitOfWork.Complete();
        //    }

        //    var domain = "https://localhost:7293/";
        //    var options = new SessionCreateOptions
        //    {

        //        LineItems = new List<SessionLineItemOptions>(),
        //        SuccessUrl = domain + $"Customer/Cart/OrderComifrim?id={ShoppingCartVM.OrderHeader.Id}",
        //        CancelUrl = domain + "Customer/Cart/Index",
        //    };

        //    foreach (var item in ShoppingCartVM.CartList)
        //    {
        //        var sessionlineOption = new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmount = (long)(item.Product.Price),
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.Product.Name
        //                }
        //            },
        //            Quantity = item.Count,
        //        };
        //        options.LineItems.Add(sessionlineOption);
        //    }

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    ShoppingCartVM.OrderHeader.SessionId = session.Id;
        //    _unitOfWork.Complete();
        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);
        //}

        [HttpPost]
        [ActionName("Sammary")]
        [ValidateAntiForgeryToken]
        public IActionResult SammaryComfirm(DTOShoppingCartSamary dTOShoppingCartSamary)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<ShoppingCart> CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUsersId == claim.Value, Includeword: "Product");

            List<DTOShoppingCart> DtoShoppingCarts = new();

            foreach (var item in CartList)
            {
                var DtoShoopingCart = new DTOShoppingCart
                {
                    Id = item.Id,
                    Product = item.Product,
                    ApplicationUsersId = item.ApplicationUsersId,
                    ProductId = item.ProductId,
                    Count = item.Count,
                };

                DtoShoppingCarts.Add(DtoShoopingCart);
            }


            ApplicationUsers application = _unitOfWork.ApplicationUsers.GetFirstOrDefualt(x => x.Id == claim.Value);

            DTOOrderHeader dTOOrderHeader = new DTOOrderHeader
            {
                ApplicationUsers = application,
                ApplicationUsersId = application.Id,
                Name = application.Name,
                Address = application.Address,
                PhoneNumber = application.PhoneNumber,
                City = application.City,
                OrderStatus = SD.Pending,
                PaymentStatus = SD.Pending,
                OrderDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(4),
                ShippingDate = DateTime.Now.AddDays(4)
            };

            dTOShoppingCartSamary.dTOShoppingCarts = DtoShoppingCarts;
            dTOShoppingCartSamary.DTOOrderHeader = dTOOrderHeader;
            

            foreach (var item in dTOShoppingCartSamary.dTOShoppingCarts)
            {
                dTOShoppingCartSamary.Total += (item.Count * item.Product.Price);
            };

            dTOShoppingCartSamary.DTOOrderHeader.TotalPrice = dTOShoppingCartSamary.Total;
            OrderHeader orderHeader = new OrderHeader
            {
                ApplicationUsers = dTOOrderHeader.ApplicationUsers,
                ApplicationUsersId = dTOOrderHeader.ApplicationUsers.Id,
                Name = dTOOrderHeader.Name,
                Address = dTOOrderHeader.Address,
                PhoneNumber = dTOOrderHeader.PhoneNumber,
                City = dTOOrderHeader.City,
                OrderStatus = dTOOrderHeader.OrderStatus,
                PaymentStatus = dTOOrderHeader.PaymentStatus,
                OrderDate = dTOOrderHeader.OrderDate,
                PaymentDate = dTOOrderHeader.PaymentDate,
                ShippingDate = dTOOrderHeader.ShippingDate,
                TotalPrice = dTOOrderHeader.TotalPrice
            };
            _unitOfWork.OrderHeader.Add(orderHeader);
            _unitOfWork.Complete();

            foreach (var item in dTOShoppingCartSamary.dTOShoppingCarts)
            {
                OrderDetails orderDetails = new OrderDetails
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = orderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Complete();
            }


            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUsersId == orderHeader.ApplicationUsersId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Complete();

            //var domain = "https://localhost:7293/";
            //     SessionCreateOptions options = new SessionCreateOptions
            //    {

            //        LineItems = new List<SessionLineItemOptions>(),
            //        SuccessUrl = domain + $"Customer/Cart/OrderComifrim?id={orderHeader.Id}",
            //        CancelUrl = domain + "Customer/Cart/Index",
            //    };

            //    foreach (var item in dTOShoppingCartSamary.dTOShoppingCarts)
            //    {
            //        var sessionlineOption = new SessionLineItemOptions
            //        {
            //            PriceData = new SessionLineItemPriceDataOptions
            //            {
            //                UnitAmount = (long)(item.Product.Price),
            //                Currency = "usd",
            //                ProductData = new SessionLineItemPriceDataProductDataOptions
            //                {
            //                    Name = item.Product.Name
            //                }
            //            },
            //            Quantity = item.Count,
            //        };
            //        options.LineItems.Add(sessionlineOption);
            //    }

            //    var service = new SessionService();
            //    Session session = service.Create(options);

            //    ShoppingCartVM.OrderHeader.SessionId = session.Id;
            //    _unitOfWork.Complete();
            //    Response.Headers.Add("Location", session.Url);
            //    return new StatusCodeResult(303);
            return RedirectToAction("OrderComfirm","Cart");
        }

        public IActionResult OrderComfirm()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
