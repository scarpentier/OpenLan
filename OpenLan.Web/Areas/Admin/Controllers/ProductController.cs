using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using OpenLan.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLan.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("ManageProducts", "Allowed")]
    public class ProductController : Controller
    {
        private readonly OpenLanContext db;

        public ProductController(OpenLanContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await db.Products.AddAsync(product, Context.RequestAborted);
                await db.SaveChangesAsync(Context.RequestAborted);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = db.Products.Where(p => p.Id == id).FirstOrDefault();

            if (product == null)
            {
                return View(product);
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.ChangeTracker.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync(Context.RequestAborted);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = db.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = db.Products.Where(p => p.Id == id).FirstOrDefault();

            if (product != null)
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync(Context.RequestAborted);
            }

            return RedirectToAction("Index");
        }
    }
}
