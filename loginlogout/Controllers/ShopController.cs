using loginlogout.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    [Authorize(Roles = "User")]

    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
       public ShopController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            this._context = context;
            this._usermanager = usermanager;
        }
        [HttpGet]
        public IActionResult Shop()
        {
            var userId = _usermanager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            ViewBag.CartItems = cartItems;

            var showproducts = _context.Products.ToList();
            var categories = _context.Category.ToList();

            ViewBag.Category = categories;
            ViewBag.CountCartitem = _context.CartItems
                           .Where(c => c.UserId == userId)
                           .Count();
            return View(showproducts);

        }
        public IActionResult ByCategory(int id)
        {
            var category = _context.Products.Where(p=>p.CategoryId == id).ToList();
           if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }



        [HttpGet]
        public IActionResult Search(string searchProduct)
        {
            var results = _context.Products
                .Where(p => p.Name.Contains(searchProduct))
                .ToList();
            var userId = _usermanager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }
            ViewBag.CountCartitem = _context.CartItems.Where(c => c.UserId == userId).Count();

            return View(results); 
        }
    }
}
