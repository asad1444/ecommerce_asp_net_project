using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            var userId = userManager.GetUserId(User);
            if(userId == null)
            {
                return NotFound();
            }
            var cartitems = context.CartItems.Include(p => p.Product).Where(c => c.UserId == userId).ToList();
            ViewBag.CartItems = cartitems;
            ViewBag.CountCartitem = context.CartItems
                           .Where(c => c.UserId == userId)
                           .Count(); return View();
        }

        [HttpPost]

        public IActionResult Contact(Contact contact)
        {
            contact.SubmittedAt = DateTime.Now;
            context.Contacts.Add(contact); 
            context.SaveChanges();

            return RedirectToAction("Contact");
        }

        public IActionResult Search(string searchProduct)
        {
            var search = context.Products.Where(p => p.Name.Contains(searchProduct));
            return View(search);
        }
    }
}
