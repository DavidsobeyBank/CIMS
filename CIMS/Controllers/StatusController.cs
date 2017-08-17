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
    public class StatusController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: Status
        public ActionResult Index()
        {
            var status = db.Status.Include(s => s.InstructionType).Include(s => s.Role);
            return View(status.ToList());
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
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusID,InstructionTypeID,Name,Description,RoleID")] Status status)
        {
            if (ModelState.IsValid)
            {
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
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", status.InstructionTypeID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", status.RoleID);
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusID,InstructionTypeID,Name,Description,RoleID")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", status.InstructionTypeID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", status.RoleID);
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
            db.Status.Remove(status);
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
