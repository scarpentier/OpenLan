using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenLan.Web.Models
{
    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole() : base() { }

        public ApplicationRole Role { get; set; }
    }
}