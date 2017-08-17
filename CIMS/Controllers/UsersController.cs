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
    public class UsersController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.Where(U => U.Active).ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,Surname,Email,Approved,Active,Username")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Approved = 0;
                user.Active  = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Name,Surname,Email,Approved,Active,Username")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                user.Active = true;
                user.Approved = 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Active = false;

            List<UserRole> results = (from UserRole in db.UserRoles
                                      where UserRole.UserID == id
                                      select UserRole).ToList();

            foreach (UserRole UR in results)
            {
                UR.Active = false;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Activate()
        {
            List<User> users = (from user in db.Users
                                where user.Active == false
                                select user).ToList();

            return View(users);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id)
        {
            User user = db.Users.Find(id);
            user.Active = true;

            //TODO reactivation check for UserRoles
            #region Reactivate
            List<UserRole> results = (from UserRole in db.UserRoles
                                      where UserRole.UserID == user.UserID
                                      select UserRole).ToList();

            foreach (UserRole UR in results)
            {
                UR.Active = true;
            }
#endregion 

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Approve()
        {
            List<User> users = (from user in db.Users
                                where user.Approved == 0
                                select user).ToList();

            return View(users);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id)
        {
            User user = db.Users.Find(id);
            user.Approved = 1;

            db.SaveChanges();
            return RedirectToAction("Index", "UserRoles");

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
