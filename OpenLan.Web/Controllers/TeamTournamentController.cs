using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Principal;
using Microsoft.Data.Entity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenLan.Web.Controllers
{
    public class TeamTournamentController : Controller
    {
        private readonly OpenLanContext db;

        public TeamTournamentController(OpenLanContext context)
        {
            db = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teamTounrament = await db.TeamTournament.Include(x => x.Clan).SingleAsync(x => x.Id == id);

            // Make sure the person editing this has the right to do it
            if (User.Identity.GetUserId() != teamTounrament.Clan.OwnerUserId)
            {
                return new HttpStatusCodeResult(403);
            }

            return View(teamTounrament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamTournament teamTournament)
        {
            if (ModelState.IsValid)
            {
                // Get previous object
                var tt = await db.TeamTournament.SingleAsync(x => x.Id == teamTournament.Id);
                tt.Name = teamTournament.Name;

                await db.SaveChangesAsync(Context.RequestAborted);
                return RedirectToAction("Manage", "Clan");
            }

            return View(teamTournament);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var teamTournament = await db.TeamTournament.Include(x => x.Clan).SingleAsync(x => x.Id == id);

            // Make sure the person editing this has the right to do it
            if (User.Identity.GetUserId() != teamTournament.Clan.OwnerUserId)
            {
                return new HttpStatusCodeResult(403);
            }

            return View(teamTournament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamTournament = await db.TeamTournament.SingleAsync(x => x.Id == id);

            db.TeamTournament.Remove(teamTournament);
            db.SaveChanges();

            return RedirectToAction("Manage", "Clan");
        }

        public async Task<IActionResult> RegisterClan(int id)
        {
            var tournament = await db.Tournaments.FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return new HttpStatusCodeResult(404);
            }

            // Get current user's clan details
            var currentUser = await db.Users.Include(x => x.Clan).SingleAsync(x => x.UserName == User.Identity.Name);

            // Make sure the user is part of a clan
            if (currentUser.Clan == null)
            {
                ViewBag.Error = "You must be part of a clan to do this operation";
                return RedirectToAction("Index");
            }

            // Make sure this user is the clan owner
            if (currentUser.Clan.OwnerUserId != currentUser.Id)
            {
                ViewBag.Error = "You must be the leader of your clan to register in a tournament";
                return RedirectToAction("Index");
            }

            var teamTournament = new TeamTournament
            {
                TournamentId = id,
                ClanId = (int)currentUser.ClanId,
                Name = currentUser.Clan.Name
            };

            await db.AddAsync(teamTournament);
            await db.SaveChangesAsync();

            return RedirectToAction("Manage", "Clan");
        }
    }
}
