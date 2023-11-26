using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DataAccess;
using MVC.DataAccess.IRebository;
using MVC.Model;
using MVC.Utiltiy;
using System.Runtime.InteropServices;

namespace MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.Category.GetAll();
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.Insert(category);
                await _unitOfWork.Save();
                TempData["success"] = "Category Created Success";
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var result  =await _unitOfWork.Category.Get(q=>q.Id==id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                await _unitOfWork.Save();
                TempData["success"] = "Category Update Success";
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var result = await _unitOfWork.Category.Get(q => q.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _unitOfWork.Category.Get(q => q.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            await _unitOfWork.Category.Delete(id);
            await _unitOfWork.Save();
            TempData["success"] = "Category Deleted Success";
            return RedirectToAction("Index");

        }
    }
}
