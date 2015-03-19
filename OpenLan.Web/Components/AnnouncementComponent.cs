using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Cache.Memory;
using OpenLan.Web.Models;

namespace OpenLan.Web.Components
{
    [ViewComponent(Name = "Announcement")]
    public class AnnouncementComponent : ViewComponent
    {
        private readonly OpenLanContext _dbContext;
        private readonly IMemoryCache _cache;

        public AnnouncementComponent(OpenLanContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _cache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestPost = await _cache.GetOrSet("latestPost", async context =>
            {
                context.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                return await GetLatestPost();
            });

            return View(latestPost);
        }

        private Task<Post> GetLatestPost()
        {
            var latestAlbum = _dbContext.Posts
                .OrderByDescending(p => p.DateCreated)
                .Where(p =>p.IsPublished)
                .FirstOrDefaultAsync();

            return latestAlbum;
        }
    }
}