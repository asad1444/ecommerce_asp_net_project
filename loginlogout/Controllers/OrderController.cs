using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;  // ✅ Fixed here

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Order()
        {
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

            return View();
        }

        [HttpPost]
        public IActionResult Order(Order order)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cartItems = _context.CartItems
                .Include(p => p.Product)
                .Where(c => c.UserId == userId)
                .ToList();

          
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;
            order.Name = cartItems.First().Product.Name;
            order.Price = cartItems.Sum(c => c.Product.Price);
            order.Quantity = cartItems.Sum(c => c.Quantity);
            order.Total = cartItems.Sum(c => c.Product.Price * c.Quantity);
           
            

            _context.Orders.Add(order);
          
            _context.SaveChanges(); // OrderId mil gaya
            cartItems.ForEach(item => item.Product.Stock -= item.Quantity);

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges(); // Save updated cart items

          
            return RedirectToAction("Order"); // Make a view for this
        }

    }
}
