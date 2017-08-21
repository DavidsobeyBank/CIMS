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

        public ActionResult Mapping(int id)
        {
            List<Status>Statuses = StatusLooper(id);
            

            if (Statuses == null)
            {
                return HttpNotFound();
            }
            return View(Statuses);
        }

        [HttpPost]
        public ActionResult Mapping(List<string> items)
        {
            for(int i = 0; i < items.Count-1; i++)
            {
                Status status = db.Status.Find(Convert.ToInt32(items[i]));
                status.NextStatus = db.Status.Find(Convert.ToInt32(items[i + 1])).StatusID;
                db.Entry(status).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Json(new { ok = true, newurl = Url.Action("Index") });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Status> StatusLooper(int id)
        {
            List<Status> Statuses = db.Status.Where(S => S.InstructionTypeID == id && S.Active).ToList();
            Status Archive = Statuses.Where(S => S.Name.Equals("Archive")).First();
            List<Status> SList = new List<Status>
            {
                Archive
            };
            int prev = Archive.StatusID;
            Statuses.Remove(Archive);
            int count = 0;
            foreach(Status S in Statuses)
            {
                if(S.NextStatus == 1)
                {
                    count++;
                }
                if (count > 1)
                {
                    Statuses.Add(Archive);
                    return Statuses;
                }
                Status next = Statuses.Find(Ss => Ss.NextStatus == prev);
                prev = next.StatusID;
                SList.Add(next);
            }
            
            SList.Reverse();
            return SList;
        }
    }
}
