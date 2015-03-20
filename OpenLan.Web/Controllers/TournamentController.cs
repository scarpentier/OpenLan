using Microsoft.AspNet.Mvc;
using System.Linq;
using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly OpenLanContext db;

        public TournamentController(OpenLanContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult IndexPartial()
        {
            return PartialView(db.Tournaments.Include(x => x.Teams));
        }
    }
}
