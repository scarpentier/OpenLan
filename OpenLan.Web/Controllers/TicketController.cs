using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenLan.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly OpenLanContext db;

        public TicketController(OpenLanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Manage()
        {
            return View(db.Tickets
                .Include(x => x.Order)
                .Include(x => x.UserAssigned)
                .Include(x => x.Seat)
                .Where(x => x.UserOwnerId == User.Identity.GetUserId()));
        }
    }
}
