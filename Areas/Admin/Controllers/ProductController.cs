using ShoplyStore.DataAccess.Data;
using ShoplyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoplyStore.DataAccess.Repository.IRepository;
using ShoplyStore.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoplyStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ShoplyStore.Utility;



namespace ShoplyStoreWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            

            List<Product> Products = _unitOfWork.Product.GetAll(IncludeProperties: "Category").ToList();
            return View(Products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList()
            };

            if (id == 0 || id == null)
            {
                // Create Product
                return View(ProductVM);
            }
            else
            {
                // Update Product
                ProductVM.Product = _unitOfWork.Product.Get(c => c.Id == id);
                if (ProductVM.Product == null)
                {
                    return NotFound();
                }
                return View(ProductVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (formFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    // Create Product
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    // Update Product
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully!";
                }

                _unitOfWork.Save();
                
                return RedirectToAction("index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList();
                return View(productVM);
            }
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(IncludeProperties: "Category");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDelete = _unitOfWork.Product.Get(c => c.Id == id);
            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var oldImagePath = Path.Combine(wwwRootPath, productToDelete.ImageUrl?.TrimStart('\\')??"");
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            TempData["success"] = "Product deleted successfully!";
            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully!" });
        }
        #endregion


    }
}
