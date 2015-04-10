using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaseType;
using Microsoft.AspNet.Identity;
using WebMTHR.Models;

namespace WebMTHR.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext = WebDbHelper.GetDbContext();
        public async Task<ActionResult> Index()
        {
            var page=new StartPageViewModel();
            page=await GetThemes(page);
            return View(page);
        }

        private async Task<StartPageViewModel> GetThemes(StartPageViewModel page)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid currentUserId = Guid.Parse(User.Identity.GetUserId());
                page.Themes =
                    await
                        (from user in _dbContext.UserRoles.Where(f => f.IdUser == currentUserId)
                            join proj in _dbContext.Projects on user.IdProject equals proj.IdProject
                            select new Theme() {Description = proj.Purpose, Name = proj.Name}
                            ).ToListAsync();
            }
            else
                page.Helper = "Авторизируйтесь! что бы получить больше информации ;)";

            return page;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}