using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;
using System.Data.Entity.SqlServer;
using PagedList;

namespace CIMS.Controllers
{
    public class InstructionsController : Controller
    {
        private CIMS_NEWEntities db = new CIMS_NEWEntities();

        // GET: Instructions
        public ActionResult Index()
        {
            var instructions = db.Instructions.Include(i => i.Branch).Include(i => i.Client).Include(i => i.Currency).Include(i => i.Currency1).Include(i => i.InstructionType).Include(i => i.User).Include(i => i.User1);
            return View(instructions.ToList());
        }

        // GET: Instructions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            return View(instruction);
        }

        // GET: Instructions/Create
        public ActionResult Create(int ID)
        {
            Client client = db.Clients.Find(ID);
            ViewBag.Client = client.Name;
            ViewBag.ClientID = Convert.ToString(client.ClientID);
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName");
            ViewBag.CurrencyFrom = new SelectList(db.Currencies, "CurrencyID", "CurrencyName");
            ViewBag.CurrencyTo = new SelectList(db.Currencies, "CurrencyID", "CurrencyName");
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name");
            ViewBag.FromUser = new SelectList(db.Users, "UserID", "Name");
            ViewBag.ToUser = new SelectList(db.Users, "UserID", "Name");
            return View();
        }

        // POST: Instructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructionTypeID,Amount,ClientID,CurrencyTo,BranchID,EERef")] Instruction instruction, string clientID, string comment)
        {
            string UN = User.Identity.Name.Split('\\').Last();

            instruction.ClientID = Convert.ToInt32(clientID);
            instruction.CreateUser = db.Users.Where(U => U.Username == UN).First().UserID;
            instruction.FromUser = db.Users.Where(U => U.Username == UN).First().UserID;
            instruction.CurrencyFrom = db.Currencies.First().CurrencyID;
            instruction.CreateDate = DateTime.Now;
            instruction.StatusID = findStatus(instruction.InstructionTypeID);
            instruction.ToUser = 1;

            db.Instructions.Add(instruction);
            db.SaveChanges();

            Models.Action action = new Models.Action();
            action.ActionDate = DateTime.Now;
            action.Comment = comment;
            action.InstructionID = db.Instructions.ToList().Last().InstructionID;
            action.StatusID = findStatus(instruction.InstructionTypeID);
            action.UserID = db.Users.Where(U => U.Username == UN).First().UserID;
            db.Actions.Add(action);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Instructions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", instruction.BranchID);
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "Name", instruction.ClientID);
            ViewBag.CurrencyFrom = new SelectList(db.Currencies, "CurrencyID", "CurrencyName", instruction.CurrencyFrom);
            ViewBag.CurrencyTo = new SelectList(db.Currencies, "CurrencyID", "CurrencyName", instruction.CurrencyTo);
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", instruction.InstructionTypeID);
            ViewBag.FromUser = new SelectList(db.Users, "UserID", "Name", instruction.FromUser);
            ViewBag.ToUser = new SelectList(db.Users, "UserID", "Name", instruction.ToUser);
            return View(instruction);
        }

        // POST: Instructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructionID,InstructionTypeID,CreateDate,CreateUser,StatusID,FromUser,ToUser,Amount,ClientID,CurrencyFrom,CurrencyTo,BranchID,EERef")] Instruction instruction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instruction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", instruction.BranchID);
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "Name", instruction.ClientID);
            ViewBag.CurrencyFrom = new SelectList(db.Currencies, "CurrencyID", "CurrencyName", instruction.CurrencyFrom);
            ViewBag.CurrencyTo = new SelectList(db.Currencies, "CurrencyID", "CurrencyName", instruction.CurrencyTo);
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes, "InstructionTypeID", "Name", instruction.InstructionTypeID);
            ViewBag.FromUser = new SelectList(db.Users, "UserID", "Name", instruction.FromUser);
            ViewBag.ToUser = new SelectList(db.Users, "UserID", "Name", instruction.ToUser);
            return View(instruction);
        }

        // GET: Instructions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instruction instruction = db.Instructions.Find(id);
            if (instruction == null)
            {
                return HttpNotFound();
            }
            return View(instruction);
        }

        // POST: Instructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instruction instruction = db.Instructions.Find(id);
            db.Instructions.Remove(instruction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParam = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParam = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var Clients = from s in db.Clients
                          where s.Active
                           select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                Clients = from s in db.Clients
                          where SqlFunctions.StringConvert((double)s.ClientID).Contains(searchString)
                          || s.Name.Contains(searchString)
                          || s.CustomerNumber.Contains(searchString)
                          || s.AccountNumber.Contains(searchString)
                          && s.Active
                          select s;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Clients = Clients.OrderByDescending(s => s.Name);
                    break;
                case "Name":
                    Clients = Clients.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    Clients = Clients.OrderByDescending(s => s.ClientID);
                    break;
                default:
                    Clients = Clients.OrderBy(s => s.ClientID);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Clients.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int findStatus(int InstructionTypeID)
        {
            List<Status> Statuses = db.Status.Where(S => S.InstructionTypeID == InstructionTypeID).ToList();
            List<int> nextS = new List<int>();

            foreach (Status s in Statuses)
            {
                nextS.Add(s.NextStatus);
            }
            foreach(Status s in Statuses)
            {
                bool check = false;
                foreach(int i in nextS)
                {
                    if(s.StatusID == i)
                    {
                        check = true;
                    }
                }
                if(!check)
                {
                    return s.StatusID;
                }
            }
            return 1;
        }
    }
}
