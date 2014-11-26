using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using OpenLan.Web.Models;
using System.Linq;
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

        public IActionResult Edit(int id)
        {
            var tournament = db.Tournaments.Where(t => t.Id == id).FirstOrDefault();

            if (tournament == null)
            {
                return View(tournament);
            }

            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.ChangeTracker.Entry(tournament).State = EntityState.Modified;
                await db.SaveChangesAsync(Context.RequestAborted);
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        public IActionResult Delete(int id)
        {
            var tournament = db.Tournaments.Where(t => t.Id == id).FirstOrDefault();
            return View(tournament);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = db.Tournaments.Where(t => t.Id == id).FirstOrDefault();

            if (tournament != null)
            {
                db.Tournaments.Remove(tournament);
                await db.SaveChangesAsync(Context.RequestAborted);
            }

            return RedirectToAction("Index");
        }
    }
}
