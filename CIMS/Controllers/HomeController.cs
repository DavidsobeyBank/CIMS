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
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return RedirectToAction("Index");
        }

        //public void Authenticated()
        //{ 
        //    CustomRoleProvider A = new CustomRoleProvider();
        //    string[] roles = A.GetRolesForUser(ID);
        //}

        
    }
}