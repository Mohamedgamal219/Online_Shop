using Microsoft.AspNetCore.Mvc;
using myShop.DataAccess.Migrations;
using myShop.Entity.DTO;
using myShop.Entity.Models;
using myShop.Entity.Repository;
using myShop.Uititlity;
namespace myShop.Web.Areas.Admin.Controllers
{
    [Area(SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<OrderHeader> orderHeaders;
             orderHeaders = _unitOfWork.OrderHeader.GetAll(Includeword: "ApplicationUsers");
            return View(orderHeaders);
        }

        public IActionResult Details(int? id)
        {
            var orderHeaders = _unitOfWork.OrderHeader.GetFirstOrDefualt(x=>x.Id == id , Includeword: "ApplicationUsers");
            IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetails.GetAll(x => x.OrderHeaderId == id, Includeword: "Product"); ;

            var dtoHeader = new DTOOrderHeader
            {
                Id = orderHeaders.Id,
                ApplicationUsers = orderHeaders.ApplicationUsers,
                ApplicationUsersId = orderHeaders.ApplicationUsers.Id,
                Name = orderHeaders.Name,
                Address = orderHeaders.Address,
                PhoneNumber = orderHeaders.PhoneNumber,
                City = orderHeaders.City,
                OrderStatus = orderHeaders.OrderStatus,
                PaymentStatus = orderHeaders.PaymentStatus,
                OrderDate = orderHeaders.OrderDate,
                PaymentDate = orderHeaders.PaymentDate,
                ShippingDate = orderHeaders.ShippingDate,
                TotalPrice = orderHeaders.TotalPrice,
                Carrier = orderHeaders.Carrier,
                TrackingNumber = orderHeaders.TrackingNumber,
                
            };
            
            List<DTOOrderDetails> details = new List<DTOOrderDetails>();

            foreach (var item in orderDetails )
            {
                DTOOrderDetails DTOorderDetails = new DTOOrderDetails
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Product = item.Product,
                    OrderHeaderId = item.OrderHeaderId,
                    Price = item.Product.Price,
                    Count = item.Count,

                    total = (item.Count * item.Product.Price)
                };
                details.Add(DTOorderDetails);
                
            }
            decimal to1 = 0;
            foreach (var item in details)
            {
                to1 += item.total;
            }

            DTOOrderDetailsSamary samary = new DTOOrderDetailsSamary
            {
                dTOOrderHeaders = dtoHeader,
                dTOOrderDetails = details,
                Total = to1
            };
            
            return View(samary);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(DTOOrderDetailsSamary dTOOrderDetailsSamary)
        {
            var orderheaderInDB = _unitOfWork.OrderHeader.GetFirstOrDefualt(x => x.Id == dTOOrderDetailsSamary.dTOOrderHeaders.Id);

            orderheaderInDB.Name = dTOOrderDetailsSamary.dTOOrderHeaders.Name;
            orderheaderInDB.Address = dTOOrderDetailsSamary.dTOOrderHeaders.Address;
            orderheaderInDB.PhoneNumber = dTOOrderDetailsSamary.dTOOrderHeaders.PhoneNumber;
            orderheaderInDB.City = dTOOrderDetailsSamary.dTOOrderHeaders.City;

            if(dTOOrderDetailsSamary.dTOOrderHeaders.Carrier!= null)
            {
                orderheaderInDB.Carrier = dTOOrderDetailsSamary.dTOOrderHeaders.Carrier;
            }
            if (dTOOrderDetailsSamary.dTOOrderHeaders.TrackingNumber != null)
            {
                orderheaderInDB.TrackingNumber = dTOOrderDetailsSamary.dTOOrderHeaders.TrackingNumber;
            }

            if (dTOOrderDetailsSamary.dTOOrderHeaders.TrackingNumber != null)
            {
                orderheaderInDB.PaymentStatus = SD.Proccessing;
                orderheaderInDB.OrderStatus = SD.Proccessing;
            }
            _unitOfWork.OrderHeader.Update(orderheaderInDB);
            _unitOfWork.Complete();

            return RedirectToAction("Details", "Order", new { orderheaderInDB.Id });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete (int? id)
        //{
        //    var orderheader = _unitOfWork.OrderHeader.GetFirstOrDefualt(x => x.Id == id);

        //    _unitOfWork.OrderHeader.Remove(orderheader);
        //    _unitOfWork.Complete();

        //    return RedirectToAction("Index", "Order");
        //}

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var orderHeaders = _unitOfWork.OrderHeader.GetFirstOrDefualt(x => x.Id == id, Includeword: "ApplicationUsers");

            var dtoHeader = new DTOOrderHeader
            {
                Id = orderHeaders.Id,
                ApplicationUsers = orderHeaders.ApplicationUsers,
                ApplicationUsersId = orderHeaders.ApplicationUsers.Id,
                Name = orderHeaders.Name,
                Address = orderHeaders.Address,
                PhoneNumber = orderHeaders.PhoneNumber,
                City = orderHeaders.City,
                OrderStatus = orderHeaders.OrderStatus,
                PaymentStatus = orderHeaders.PaymentStatus,
                OrderDate = orderHeaders.OrderDate,
                PaymentDate = orderHeaders.PaymentDate,
                ShippingDate = orderHeaders.ShippingDate,
                TotalPrice = orderHeaders.TotalPrice,
                Carrier = orderHeaders.Carrier,
                TrackingNumber = orderHeaders.TrackingNumber,

            };

            DTOOrderDetailsSamary samary = new DTOOrderDetailsSamary
            {
                dTOOrderHeaders = dtoHeader,
               
            };

            return View(samary);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ComfirmDelete(int? id)
        {
            var OrdereHeader = _unitOfWork.OrderHeader.GetFirstOrDefualt(x => x.Id == id);

            if (OrdereHeader == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderHeader.Remove(OrdereHeader);
            _unitOfWork.Complete();
            TempData["Delete"] = "Item Has Deleted Scussfully";
            return RedirectToAction("Index");

        }
    }
}
