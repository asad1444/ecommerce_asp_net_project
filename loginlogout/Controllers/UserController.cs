using loginlogout.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager; 
public UserController(ApplicationDbContext context,UserManager<ApplicationUser> userManager) { 
            this._context = context;
            this._userManager = userManager;
        
        }

        public IActionResult Profile()
        {
            var products = _context.Products.Take(8).ToList();        
            var categories = _context.Category.ToList();
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            ViewBag.CartItems = cartItems;
            ViewBag.Category = categories;
            ViewBag.Cartcount = _context.CartItems
                   .Where(c => c.UserId == userId)
                   .Count();

         
            return View(products); 
        }

           public IActionResult ByCategory(int id) {

            var category = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (category == null)
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
            var userId = _userManager.GetUserId(User);
            if(userId == null)
            {
                return Unauthorized();
            }
            ViewBag.CountCartitem = _context.CartItems.Where(c=> c.UserId == userId).Count();
            return View(results);
        }

      
    }
}
