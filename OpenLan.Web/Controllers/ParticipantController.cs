using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenLan.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly OpenLanContext db;

        public ParticipantController(OpenLanContext context)
        {
            db = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(db.Users.Include(x => x.Clan));
        }
    }
}
