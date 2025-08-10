using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    [Authorize]
    public class ShowCartItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShowCartItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult ShowCartItem()
        {
            var UserId = _userManager.GetUserId(User);
            var show = _context.CartItems.Include(p => p.Product).Where(c => c.UserId == UserId)
.ToList();
            ViewBag.CountCartitem = _context.CartItems
                           .Where(c => c.UserId == UserId)
                           .Count(); return View(show);  
        }

        [HttpPost]
        public IActionResult ShowCartItem(int id, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(); // user not logged in
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // product not found
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.ProductId == id && c.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.ProductId,
                    UserId = userId,
                    Quantity = quantity,
                    Product = product
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
            return RedirectToAction("ShowCartItem");
        }
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var remove = _context.CartItems.FirstOrDefault(c=> c.ProductId == id);
            if(remove != null)
            {
                _context.CartItems.Remove(remove);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowCartItem");

        }

    }
}
