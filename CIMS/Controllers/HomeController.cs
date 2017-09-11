using CIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CIMS.Controllers
{
    public class HomeController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();
        [Authorize]
        public ActionResult Index()
        {
            CustomRoleProvider CRP = new CustomRoleProvider();
            string a = CRP.GetANumber(username: Request.LogonUserIdentity.Name);
            string[] Roles = CRP.GetRolesForUser(a);
            List<Role> RoleList = new List<Role>();
            foreach (string R in Roles)
            {
                try
                {
                    RoleList.AddRange(db.Roles.Where(Ro => Ro.RoleName.Equals('R')).ToList());
                }
                catch(Exception)
                {

                }
            }
            return View(RoleList);
        }

        public ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Auth(string username)
        {
            CustomRoleProvider CRP = new CustomRoleProvider();
            CRP.GetRolesForUser(username);
            return RedirectToAction("index");
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