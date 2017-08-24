using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;

namespace CIMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserRolesController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int id)
        {
            List<ViewModel.UserRoleViewModel> UserRoles = new List<ViewModel.UserRoleViewModel>();
            foreach (Role R in db.Roles)
            {
                int result = (from UserRole in db.UserRoles
                              where UserRole.UserID == id && UserRole.RoleID == R.RoleID
                              select UserRole).Count();
                if (result == 1)
                {
                    ViewModel.UserRoleViewModel userRole = new ViewModel.UserRoleViewModel()
                    {
                        User = db.Users.Find(id),
                        Role = R,
                        active = true
                    };
                    UserRoles.Add(userRole);
                }
                else if (result == 0)
                {
                    ViewModel.UserRoleViewModel userRole = new ViewModel.UserRoleViewModel()
                    {
                        User = db.Users.Find(id),
                        Role = R,
                        active = false
                    };
                    UserRoles.Add(userRole);
                }
            }
            return View(UserRoles);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int UID, int RID, bool A)
        {
            int result = (from UserRole in db.UserRoles
                          where UserRole.UserID == UID && UserRole.RoleID == RID
                          select UserRole).Count();
            if(result == 1)
            {
                UserRole userRole = (from UserRole in db.UserRoles
                                     where UserRole.UserID == UID && UserRole.RoleID == RID
                                     select UserRole).First();
                if(A)
                {
                    db.UserRoles.Remove(userRole);
                    db.SaveChanges();
                }
            }
            else if(result == 0)
            {
                UserRole userRole = new UserRole()
                {
                    UserID = UID,
                    RoleID = RID,
                    Active = true,
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }
            CIMS.Models.CustomRoleProvider RP = new CustomRoleProvider();
            RP.GetRolesForUser(User.Identity.Name.Split('\\').Last());
            return RedirectToAction("Edit/"+UID, "UserRoles");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
