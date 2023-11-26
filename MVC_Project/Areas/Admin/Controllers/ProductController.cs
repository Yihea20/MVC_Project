using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.IRebository;
using MVC.Model;
using MVC.Model.DTOs;
using MVC.Model.VM;
using MVC.Utiltiy;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _environment;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.Product.GetAll(include:q=>q.Include(x=>x.Category));
            return View(result);
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM product = new();
            var CategoryList = await _unitOfWork.Category.GetAll();
            var result = _mapper.Map<IList<CategoryDTO>>(CategoryList);

            if (id == null || id == 0)
            {
                product.Categorys = result.Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
                product.Product = new ProductDTO();
                return View(product);
            }
            else
            {
                var productt = await _unitOfWork.Product.Get(q => q.Id == id);
                var results = _mapper.Map<ProductDTO>(productt);
                product.Categorys = result.Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
                product.Product = new ProductDTO();
                product.Product = results;
                productt.Used = true;
                _unitOfWork.Product.Update(productt);
                await _unitOfWork.Save(); 
                return View(product);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductDTO Product,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rote = _environment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(rote, @"images\product");
                    if(!string.IsNullOrEmpty(Product.ImageUrl))
                    {
                        var old = Path.Combine(rote, Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(old))
                        {
                            System.IO.File.Delete(old);
                        }
                    }

                    using (var FileStream =new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(FileStream);
                    }
                    Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (Product.Id != 0)
                {
                    var map = _mapper.Map<Product>(Product);
                    map.Used = false;
                    _unitOfWork.Product.Update(map);
                    await _unitOfWork.Save();
                    TempData["success"] = "Category Update Success";
                }
                else
                {
                    
                    var map = _mapper.Map<Product>(Product);
                    await _unitOfWork.Product.Insert(map);
                    await _unitOfWork.Save();
                    TempData["success"] = "Category Created Success";
                }
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
            var result = await _unitOfWork.Product.Get(q => q.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _unitOfWork.Product.Get(q => q.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(result.ImageUrl))
            {
                var old = Path.Combine(_environment.WebRootPath, result.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(old))
                {
                    System.IO.File.Delete(old);
                }
            }
            await _unitOfWork.Product.Delete(id);
            await _unitOfWork.Save();
            TempData["success"] = "Category Deleted Success";
            return RedirectToAction("Index");

        }
    }
}

