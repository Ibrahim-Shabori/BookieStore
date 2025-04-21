using ShoplyStore.DataAccess.Repository.IRepository;
using ShoplyStore.Models.ViewModels;
using ShoplyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoplyStore.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ShoplyStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> Companies = _unitOfWork.Company.GetAll().ToList();
            return View(Companies);
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();

            if (id == 0 || id == null)
            {
                // Create Product
                return View(company);
            }
            else
            {
                // Update Product
                company = _unitOfWork.Company.Get(c => c.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    // Create Product
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company created successfully!";
                }
                else
                {
                    // Update Product
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company updated successfully!";
                }

                _unitOfWork.Save();

                return RedirectToAction("index");
            }
            else
            {
                return View(company);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToDelete = _unitOfWork.Company.Get(c => c.Id == id);
            if (companyToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            
            _unitOfWork.Company.Remove(companyToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully!";
            return Json(new { success = true, message = "Company deleted successfully!" });
        }
        #endregion

    }
}
