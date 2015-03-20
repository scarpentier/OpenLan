using Microsoft.AspNet.Mvc;
using OpenLan.Web.Models;
using System.Threading.Tasks;

namespace OpenLan.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly OpenLanContext db;

        public StoreController(OpenLanContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(db.Products);
        }

        public async Task<IActionResult> Browse()
        {
            return View(db.Products);
        }
    }
}
