using CIMS.Models;
using System.Web.Mvc;

namespace CIMS.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            CustomRoleProvider test = new CustomRoleProvider();
            string a = test.GetANumber(User.Identity.Name);
            return View();
        }
        
        public ActionResult Switch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Switch(string username)
        {
            CustomRoleProvider CRP = new CustomRoleProvider();
            CRP.GetRolesForUser(username);
            return RedirectToAction("index");
        }

    }
}