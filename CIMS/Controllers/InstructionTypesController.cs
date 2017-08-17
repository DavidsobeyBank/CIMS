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
    public class InstructionTypesController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: InstructionTypes
        public ActionResult Index()
        {
            return View(db.InstructionTypes.Where(I => I.Active).ToList());
        }

        // GET: InstructionTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionType instructionType = db.InstructionTypes.Find(id);
            if (instructionType == null)
            {
                return HttpNotFound();
            }
            return View(instructionType);
        }

        // GET: InstructionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InstructionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructionTypeID,Name,Description,Cutoff,Active")] InstructionType instructionType)
        {
            if (ModelState.IsValid)
            {
                instructionType.Active = true;
                db.InstructionTypes.Add(instructionType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructionType);
        }

        // GET: InstructionTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionType instructionType = db.InstructionTypes.Find(id);
            if (instructionType == null)
            {
                return HttpNotFound();
            }
            return View(instructionType);
        }

        // POST: InstructionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructionTypeID,Name,Description,Cutoff,Active")] InstructionType instructionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructionType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructionType);
        }

        // GET: InstructionTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionType instructionType = db.InstructionTypes.Find(id);
            if (instructionType == null)
            {
                return HttpNotFound();
            }
            return View(instructionType);
        }

        // POST: InstructionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructionType instructionType = db.InstructionTypes.Find(id);
            db.InstructionTypes.Remove(instructionType);
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
