using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.IRebository;
using MVC.Model;
using System.Diagnostics;

namespace MVC_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task< IActionResult> Index()
        {
           var  productlist = await _unitOfWork.Product.GetAll(include: q => q.Include(x => x.Category));
            return View(productlist);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _unitOfWork.Product.Get(q=>q.Id==id,include: q => q.Include(x => x.Category));
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}