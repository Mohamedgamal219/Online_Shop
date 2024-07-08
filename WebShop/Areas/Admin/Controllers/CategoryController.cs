using Microsoft.AspNetCore.Mvc;
using myShop.Entity.DTO;
using myShop.Entity.Models;
using myShop.Entity.Repository;

namespace myShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DTOCategory Dtocategory)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = 0,
                    Name = Dtocategory.Name,
                    Description = Dtocategory.Description
                };

                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["Create"] = "Item Has Create Scussfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(Dtocategory);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);

            var Dtocategory = new DTOCategory
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(Dtocategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DTOCategory Dtocategory)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = Dtocategory.Id,
                    Name = Dtocategory.Name,
                    Description = Dtocategory.Description,
                    CreatedTime = DateTime.Now
                };
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["Update"] = "Item Has Updated Scussfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(Dtocategory);
            }
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteComfirm(int? id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            TempData["Delete"] = "Item Has Deleted Scussfully";
            return RedirectToAction("Index");

        }
    }
}
