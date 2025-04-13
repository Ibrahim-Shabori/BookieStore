using BookieStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookieStore.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationContext _context;
        public CategoryController(ApplicationContext context) => _context = context;
        public IActionResult Index()
        {
            List<Category> Categories = _context.Categories.ToList();
            return View(Categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

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

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            Category? existingCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            _context.Categories.Update(existingCategory);
            _context.SaveChanges();

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

            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(existingCategory);
            _context.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("index");
        }


    }
}
