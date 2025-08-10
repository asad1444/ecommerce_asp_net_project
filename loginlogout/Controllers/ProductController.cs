using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext context,IWebHostEnvironment env) {
            this._context = context;
            this._env = env;    
        }

        [HttpGet]
        public IActionResult Addproduct()
        {
            ViewBag.Category = new SelectList(_context.Category, "CategoryId", "Name");
           
            return View();
        }

        [HttpPost]
        public IActionResult Addproduct(ProductVM prod,string Name)
        {

            var checkproduct = _context.Products.FirstOrDefault(p => p.Name == Name);
            if (checkproduct != null)
            {
                TempData["errorproduct"] = "This Product is already added in the database";
                return RedirectToAction("Addproduct");
            }
            string filename = "";
            if(prod.ProductPhoto != null)
            {
                string foldername = Path.Combine(_env.WebRootPath, "ProductImages");
                filename = prod.ProductPhoto.FileName;
                string filepath = Path.Combine(foldername, filename);
                prod.ProductPhoto.CopyTo(new FileStream(filepath, FileMode.Create));

                Product pro = new Product()
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                    ProductImage = filename,
                    Stock = prod.Stock,
                    LongDescription = prod.LongDescription,
                    CategoryId = prod.CategoryId,
                   

                };
              
                _context.Products.Add(pro);
                TempData["productadd"] = "Product Added Successfully";
                _context.SaveChanges();
                return RedirectToAction("Showproducts");

            }


            return View();
        }

        public IActionResult Showproducts()
        {
            var show = _context.Products.Include(c => c.Category).ToList();
            return View(show);
        }

        [HttpGet]
        public IActionResult Edit(int id)

        {
            var find = _context.Products.Find(id);
            if(find == null)
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
            if(find == null)
            {
                return NotFound();
            }
            if(pro.ProductPhoto!= null)
            {
                string folder = Path.Combine(_env.WebRootPath, "ProductImages");
                string filename = pro.ProductPhoto.FileName;
                string filepath = Path.Combine(folder, filename);   
                pro.ProductPhoto.CopyTo(new FileStream(filepath,FileMode.Create));

                find.ProductImage = filename;
            }
            find.ProductId = pro.ProductId; 
            find.Name = pro.Name;
            find.Description = pro.Description; 
            find.Price = pro.Price;
            find.Stock = pro.Stock;
            find.LongDescription = pro.LongDescription;

            _context.Products.Update(find);
            TempData["productupdate"] = "Product Updated Successfully";
            _context.SaveChanges();
            return RedirectToAction("Showproducts");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var delete = _context.Products.Include(c=>c.Category).FirstOrDefault(p=>p.ProductId == id);
            if(delete == null)
            {
                return NotFound();
            }
            return View(delete);
        }
        [HttpPost,ActionName("DeleteProduct")]
        public IActionResult DeleteProducts(int id)
        {
            var remove = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (remove != null)
            {
                _context.Products.Remove(remove);
                TempData["Removeproduct"] = "Product Deleted Successfully";
                _context.SaveChanges();
                return RedirectToAction("ShowProducts");
            }
            return View();
        }
    }
}
