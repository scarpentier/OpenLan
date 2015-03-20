using Microsoft.AspNet.Mvc;
using System.Linq;
using OpenLan.Web.Models;
using System.Threading.Tasks;

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
        public IActionResult Index()
        {
            return View(db.Tournaments.Include(x => x.Teams));
        }

        [AllowAnonymous]
        public IActionResult IndexPartial()
        {
            return PartialView(db.Tournaments.Include(x => x.Teams));
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
            };

            await db.AddAsync(teamTournament);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
