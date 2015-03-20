using Microsoft.AspNet.Mvc;
using System.Linq;
using OpenLan.Web.Models;
using System.Threading.Tasks;
using System.Security.Principal;

namespace OpenLan.Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly OpenLanContext db;

        public TournamentController(OpenLanContext context)
        {
            db = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // If the current user is the clan owner, we'll show additional buttons
            ViewBag.IsClanOwner = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await db.Users.Include(x => x.Clan).SingleAsync(x => x.UserName == User.Identity.Name);
                if (user.ClanId != null && user.Clan.OwnerUserId == User.Identity.GetUserId())
                    ViewBag.IsClanOwner = true;
            }

            return View(db.Tournaments.Include(x => x.Teams));
        }
    }
}
