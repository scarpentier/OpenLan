using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Threading.Tasks;

namespace OpenLan.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("ManageTournaments", "Allowed")]
    public class TournamentController : Controller
    {
        private readonly OpenLanContext db;

        public TournamentController(OpenLanContext context)
        {
            db = context;
        }

        // GET: /<controller>/

        public IActionResult Index()
        {
            return View(db.Tournaments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                await db.Tournaments.AddAsync(tournament, Context.RequestAborted);
                await db.SaveChangesAsync(Context.RequestAborted);
                return RedirectToAction("Index");
            }

            return View(tournament);
        }
    }
}
