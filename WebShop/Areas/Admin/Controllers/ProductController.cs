using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myShop.Entity.DTO;
using myShop.Entity.Models;
using myShop.Entity.Repository;
using myShop.Entity.ViewModels;
using SQLitePCL;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(Includeword:"Category");
            return View(products);
        }
        //[HttpGet]
        //public IActionResult Create()
        //{ 
        //    var productVM = new ProductVM
        //    {
        //        Product = new Product(),
        //        CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.Id.ToString()
        //        })
        //    };
        //    return View(productVM);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            var product = new DTOProduct {
            CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            };
            return View(product);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ProductVM productVM,IFormFile file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var rootpath = _webHostEnvironment.WebRootPath;

        //        if (file != null)
        //        {
        //            var FileName = Guid.NewGuid().ToString();
        //            var Upload = Path.Combine(rootpath, @"Image\Products");
        //            var Ext = Path.GetExtension(file.FileName);

        //            using (var filestream = new FileStream(Path.Combine(Upload ,FileName + Ext), FileMode.CreateNew))
        //            {
        //                file.CopyTo(filestream);
        //            }
        //            productVM.Product.Img = @"Image\Products\" + FileName + Ext;
        //        }
        //        _unitOfWork.Product.Add(productVM.Product);
        //        _unitOfWork.Complete();
        //        TempData["Create"] = "Item Has Create Scussfully";

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View(productVM);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( DTOProduct Dtoproduct, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var rootpath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    var FileName = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(rootpath, @"Image\Products");
                    var Ext = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(Upload, FileName + Ext), FileMode.CreateNew))
                    {
                        file.CopyTo(filestream);
                    }
                    Dtoproduct.Img = @"Image\Products\" + FileName + Ext;
                }

                Product product = new Product
                {
                    Id = 0,
                    Name = Dtoproduct.Name,
                    Description = Dtoproduct.Description,
                    Price = Dtoproduct.Price,
                    Img = Dtoproduct.Img,
                    CategoryId = Dtoproduct.CategoryId
                };
                _unitOfWork.Product.Add(product);
                _unitOfWork.Complete();
                TempData["Create"] = "Item Has Create Scussfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(Dtoproduct);
            }
        }

        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var productVM = new ProductVM
        //    {
        //        Product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id),
        //        CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.Id.ToString()
        //        })
        //    };
        //    return View(productVM);           
        //}


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id);

            var dtoproduct = new DTOProduct
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Img = product.Img,
                CategoryId = product.CategoryId,

                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(dtoproduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DTOProduct Dtoproduct, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var rootpath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    var FileName = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(rootpath, @"Image\Products");
                    var Ext = Path.GetExtension(file.FileName);

                    var oldImg = Path.Combine(_webHostEnvironment.WebRootPath,Dtoproduct.Img.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImg))
                    {
                        System.IO.File.Delete(oldImg);
                    }

                    using (var filestream = new FileStream(Path.Combine(Upload, FileName + Ext), FileMode.CreateNew))
                    {
                        file.CopyTo(filestream);
                    }
                    Dtoproduct.Img = @"Image\Products\" + FileName + Ext;
                }
                Product product = new Product
                {
                    Id = 0,
                    Name = Dtoproduct.Name,
                    Description = Dtoproduct.Description,
                    Price = Dtoproduct.Price,
                    Img = Dtoproduct.Img,
                    CategoryId = Dtoproduct.CategoryId
                };
                _unitOfWork.Product.Add(product);

                _unitOfWork.Complete();
                TempData["Update"] = "Item Has Updated Scussfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(Dtoproduct);
            }
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id, Includeword: "Category");
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteComfirm(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id, Includeword: "Category");

            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(product);
            var oldImg = Path.Combine(_webHostEnvironment.WebRootPath, product.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldImg))
            {
                System.IO.File.Delete(oldImg);
            }
            _unitOfWork.Complete();
            TempData["Delete"] = "Item Has Deleted Scussfully";
            return RedirectToAction("Index");

        }
    }
}
