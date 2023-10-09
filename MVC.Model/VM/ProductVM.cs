using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Model.VM
{
    public class ProductVM
    {
        public ProductDTO Product { get; set; }
        public IEnumerable<SelectListItem> Categorys { get; set; }
    }
}
