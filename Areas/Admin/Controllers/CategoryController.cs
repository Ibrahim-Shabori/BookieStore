using ShoplyStore.DataAccess.Data;
using ShoplyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoplyStore.DataAccess.Repository.IRepository;
using ShoplyStore.DataAccess.Repository;
using ShoplyStore.Utility;
using Microsoft.AspNetCore.Authorization;



namespace ShoplyStoreWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> Categories = _unitOfWork.Category.GetAll().ToList();
            return View(Categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            if (category.Description == "Test")
            {
                ModelState.AddModelError("", "Description cannot be 'Test'");
            }

            if (ModelState.IsValid)
            {
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("index");
            }else return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.Category.Get(c => c.Id == id); 

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            Category? existingCategory = _unitOfWork.Category.Get(c => c.Id == category.Id); 
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            _unitOfWork.Category.Update(existingCategory);
            _unitOfWork.Save();

            if (category.Description == "Test")
            {
                ModelState.AddModelError("", "Description cannot be 'Test'");
            }

            if (ModelState.IsValid)
            {
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("index");
            }
            else return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.Category.Get(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? existingCategory = _unitOfWork.Category.Get(c => c.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(existingCategory);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("index");
        }


    }
}
