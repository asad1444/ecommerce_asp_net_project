using System.ComponentModel;
using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        [HttpGet]
        public IActionResult AddCategory()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(CategoryVM cat)
        {

            var categroy = _context.Category.FirstOrDefault(c => c.Name == cat.Name);
            if (categroy != null)
            {
                TempData["errorcategory"] = "This Category is Already Added in the database";
                return RedirectToAction("AddCategory");
            }
            string filename = "";
          
            if (cat.CategoryPhoto != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "CategoryImages");
                filename = cat.CategoryPhoto.FileName;
                string filepath = Path.Combine(folder, filename);
                cat.CategoryPhoto.CopyTo(new FileStream(filepath, FileMode.Create));

                Category c = new Category()
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name,
                    CategoryImage = filename
                };
                _context.Category.Add(c);
                TempData["addcategory"] = "Category Added Successfully";
                _context.SaveChanges();
                return RedirectToAction("ShowCategory");
            }
            return View();
        }

        public IActionResult ShowCategory()
        {
            var show = _context.Category.ToList();
            return View(show);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var show = _context.Category.Find(id);
            if (show == null)
            {
                return NotFound();
            }
            CategoryVM cat = new CategoryVM()
            {
                CategoryId = show.CategoryId,
                Name = show.Name,
            };
            ViewBag.CurrentImage = show.CategoryImage;
            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(CategoryVM cat)
        {
            var find = _context.Category.Find(cat.CategoryId);
            if (find == null)
            {
                return NotFound();
            }
            
            if (cat.CategoryPhoto != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "CategoryImages");
                string filename = cat.CategoryPhoto.FileName;
                string filepath = Path.Combine(folder, filename);
                cat.CategoryPhoto.CopyTo(new FileStream(filepath, FileMode.Create));
                find.CategoryImage = filename;

            }

            find.CategoryId = cat.CategoryId;
            find.Name = cat.Name;
            

            _context.Category.Update(find);
            TempData["updatecategory"] = "Update Category Successfully";
            _context.SaveChanges();
            return RedirectToAction("ShowCategory");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var find = _context.Category.Find(id);
            if (find == null)
            {
                return NotFound();  
            }

            return View(find);
        }
        [HttpPost,ActionName("Delete")]  
        public IActionResult DeleteConfirmed(int id)
        {
            var find = _context.Category.Find(id);
            if (find == null)
            {
                return NotFound();  
            }
            _context.Category.Remove(find);
            TempData["removecategory"] = "Category Deleted Successfully";
            _context.SaveChanges();
            return RedirectToAction("ShowCategory");
        }




    }
}