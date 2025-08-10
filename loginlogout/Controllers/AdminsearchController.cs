using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace loginlogout.Controllers
{
    public class AdminsearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
      public AdminsearchController(ApplicationDbContext context,IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }

        [HttpGet]
         public IActionResult SearchProduct()
        {
            var select = _context.Products.ToList();
            return View(select);
        }
        [HttpPost]
        public IActionResult SearchProduct(string name,string category)
        {
            var product = _context.Products.Where(p => p.Name.Contains(name)).ToList();
            return View(product);
        }
        [HttpGet]

        public IActionResult DeleteProduct(int id)
        {
            var delete = _context.Products.Include(c => c.Category).FirstOrDefault(p => p.ProductId == id);
            if (delete == null)
            {
                return NotFound();
            }
            return View(delete);
        }

        [HttpPost,ActionName("DeleteProduct")]
        public IActionResult DeleteProducts(int id)
        {
            var remove = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if(remove != null)
            {
                _context.Products.Remove(remove);
                _context.SaveChanges();
                return RedirectToAction("SearchProduct");
            }
            return View(remove);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var find = _context.Products.Find(id);
            if (find == null)
            {
                return NotFound();
            }
            ProductVM prod = new ProductVM()
            {
                ProductId = find.ProductId,
                Name = find.Name,
                Description = find.Description,
                Price = find.Price,
                Stock = find.Stock,
                LongDescription = find.LongDescription
            };
            ViewBag.CurrentImage = find.ProductImage;

            return View(prod);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM pro)
        {
            var find = _context.Products.Find(pro.ProductId);
            if (find == null)
            {
                return NotFound();
            }
            if (pro.ProductPhoto != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "ProductImages");
                string filename = pro.ProductPhoto.FileName;
                string filepath = Path.Combine(folder, filename);
                pro.ProductPhoto.CopyTo(new FileStream(filepath, FileMode.Create));

                find.ProductImage = filename;
            }
            find.ProductId = pro.ProductId;
            find.Name = pro.Name;
            find.Description = pro.Description;
            find.Price = pro.Price;
            find.Stock = pro.Stock;
            find.LongDescription = pro.LongDescription;

            _context.Products.Update(find);
            _context.SaveChanges();
            return RedirectToAction("SearchProduct");
        }

    }
}
