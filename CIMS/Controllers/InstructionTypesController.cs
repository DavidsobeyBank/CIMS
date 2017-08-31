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
    public class InstructionTypesController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: InstructionTypes
        public ActionResult Index()
        {
            return View(db.InstructionTypes.Where(I => I.Active && I.InstructionTypeID != 1).ToList());
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
            Status S = db.Status.Find(Convert.ToInt32(items.ElementAt(items.Count-1).ToString()));
            S.NextStatus = 1;
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

        public List<Status> StatusLooper(int id)
        {
            try
            {
                List<Status> Statuses = db.Status.Where(S => S.InstructionTypeID == id && S.Active).ToList();
                
                if (Statuses.Where(S => S.NextStatus == 1).Count() > 1)
                    return Statuses;
                if (Statuses.Count == 1)
                    return Statuses;

                int NextStatus = 1;
                bool flag = true;
                List<Status> StatusOrder = new List<Status>();
                foreach (Status S in Statuses)
                {
                    if(NextStatus == 1)
                    {
                        foreach (Status SS in Statuses)
                        {
                            if (SS.NextStatus == NextStatus)
                            {
                                StatusOrder.Add(SS);
                                NextStatus = SS.StatusID;
                                flag = false;
                            }
                        }
                    }
                    if(!flag && Statuses.Count != StatusOrder.Count)
                    {
                        Status status = db.Status.Where(SS => SS.NextStatus == NextStatus).First();
                        StatusOrder.Add(status);
                        NextStatus = status.StatusID;
                    }
                }
                StatusOrder.Reverse();
                return StatusOrder;
            }
            catch(Exception)
            {
                List<Status> Statuses = db.Status.Where(S => S.InstructionTypeID == id && S.Active).ToList();
                return Statuses;
            }
        }
    }
}
