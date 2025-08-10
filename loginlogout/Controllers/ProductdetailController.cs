using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    public class ProductdetailController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> usermanager;
        

        public ProductdetailController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.usermanager = userManager;
        }

        public IActionResult Productdetail(int id)
        {
            var detail = context.Products
                .Include(cat => cat.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (detail == null)
            {
                return NotFound();
            }

            var relatedProducts = context.Products
                .Where(p => p.CategoryId == detail.CategoryId && p.ProductId != id)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            var categories = context.Category.ToList();
            var userId = usermanager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var cartItems = context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            ViewBag.CartItems = cartItems;

            ViewBag.CountCartitem = context.CartItems
               .Where(c => c.UserId == userId)
               .Count();

            return View(detail);
        }


        public IActionResult AddReview()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            var find = context.Reviews.FirstOrDefault(r => r.ReviewId == review.ReviewId);
            if (find == null)
            {
                return NotFound();
            }
            context.Reviews.Add(review);
            context.SaveChanges();
            return RedirectToAction("Productdetail");

        }

    }
}
