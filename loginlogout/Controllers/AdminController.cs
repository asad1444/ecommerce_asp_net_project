using loginlogout.Areas.Identity.Data;
using loginlogout.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace loginlogout.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;  

        public AdminController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = context.Users.Count();
            ViewBag.TotalOrders = context.Orders.Count();
            ViewBag.TotalRevenue = context.Orders.Sum(o => o.Total);
            var showorder = context.Orders.ToList();

            return View(showorder);
        }

        [HttpGet]
        public IActionResult EditOrder(int id)
        {
            var findorder = context.Orders.Find(id);
            if (findorder == null)
            {
                return NotFound();
            }
            var users = context.Users.ToList();
            ViewBag.UserId = new SelectList(context.Users, "Id", "Email");

            return View(findorder);

        }
        [HttpPost]
        public IActionResult EditOrder(Order order)
        {
            var editorder = context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
            if (editorder == null)
            {
                return NotFound();
            }
            editorder.UserId = order.UserId;
            editorder.FirstName = order.FirstName;
            editorder.LastName = order.LastName;
            editorder.Address = order.Address;
            editorder.City = order.City;
            editorder.Email = order.Email;
            editorder.Status = order.Status;




            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult DeleteOrder(int id)
        {
            var finddeleteorder = context.Orders.Include(u=>u.User).FirstOrDefault(o=>o.OrderId == id);
            if(finddeleteorder == null)
            {
                return NotFound();
            }
            return View(finddeleteorder); 
        }

        [HttpPost,ActionName("DeleteOrder")]
        public IActionResult DeleteOrders(int id)
        {
            var deleteOrder = context.Orders.Find(id);
            if(deleteOrder == null)
            {
                return NotFound();
            }
            context.Orders.Remove(deleteOrder);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public IActionResult Search(string Searchstatus)
        {
            var searchResults = context.Orders
                .Where(o => o.Name.Contains(Searchstatus) && o.Status.Contains(Searchstatus))
                .ToList();
            return View(searchResults);
        }

    }
}
