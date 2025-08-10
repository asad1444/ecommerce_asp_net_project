using loginlogout.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace loginlogout.Controllers
{
    public class ShowContactController : Controller
    {
        private readonly ApplicationDbContext context;

        public ShowContactController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult ShowContact()
        {
            var showcontact = context.Contacts.ToList();
            return View(showcontact);
        }
    }
}
