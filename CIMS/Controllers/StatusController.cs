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
    public class StatusController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: Status
        public ActionResult Index()
        {
            return View(db.Status.Where(S => S.Active && S.StatusID != 1).ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes.Where(I => I.Active && I.InstructionTypeID != 1), "InstructionTypeID", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusID,InstructionTypeID,Name,Description,Active,RoleID")] Status status)
        {
            if (ModelState.IsValid)
            {
                status.Active = true;
                status.NextStatus = 1;
                db.Status.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", status.InstructionTypeID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", status.RoleID);
            return View(status);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            List<Status> Statuses = new List<Status>();
            foreach(Status S in db.Status.ToList())
            {
                if(status.InstructionTypeID == S.InstructionTypeID)
                {
                    Statuses.Add(S);
                }
            }
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes.Where(I => I.Active && I.InstructionTypeID != 1), "InstructionTypeID", "Name", status.InstructionTypeID);
            ViewBag.NextStatus = new SelectList(Statuses.Where(I => I.Active), "StatusID", "Name", status.NextStatus);
            ViewBag.RoleID = new SelectList(db.Roles.Where(I => I.Active), "RoleID", "RoleName", status.RoleID);
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusID,InstructionTypeID,RoleID,Name,Description,Active,NextStatus")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                status.Active = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", status.InstructionTypeID);
            return View(status);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status status = db.Status.Find(id);
            status.Active = false;

            List<Status> results = (from Status in db.Status
                                      where Status.NextStatus == id
                                      select Status).ToList();

            foreach (Status s in results)
            {
                s.NextStatus = status.NextStatus;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Activate()
        {
            List<Status> Status = (from status in db.Status
                                where status.Active == false && status.InstructionTypeID != 1
                                select status).ToList();

            return View(Status);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(int id)
        {
            Status status = db.Status.Find(id);
            status.Active = true;

            db.SaveChanges();
            return RedirectToAction("Index");
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
