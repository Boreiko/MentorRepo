using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Logging;
using MvcMusicStore.Infrastructure;
using MvcMusicStore.Interfaces;
using MvcMusicStore.Models;
using NLog;
using PerformanceCounterHelper;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        // GET: /Home/

        private readonly ILogger_MusicStore logger;
         public HomeController(ILogger_MusicStore logger)
        {
            this.logger = logger;
            this.logger.LogInfo("Home Controller created succesfully");
        }
        public async Task<ActionResult> Index()
        {
            logger.LogDebug("Enter to home page");

            using (var counterHelper = PerformanceHelper.CreateCounterHelper<Counters>("MvcMusicStore"))
            {
                counterHelper.Increment(Counters.Login);
            }

            return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}