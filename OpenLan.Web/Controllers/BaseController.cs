using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

using OpenLan.Web.Models;

namespace OpenLan.Web.Controllers
{
    public class BaseController : Controller
    {
        protected OpenLanContext db = new OpenLanContext();

        protected string Language { get; private set; }

        protected StrongSettings Settings { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.Language = this.Request.UserLanguages == null ? "en" : this.Request.UserLanguages[0];
            this.ViewBag.language = this.Language;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(this.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.Language);

            this.Settings = StrongSettings.GetSettings(this.db.Settings.ToList());
            this.ViewBag.settings = this.Settings;
        }
    }
}