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
        public ActionResult Create(int ID, int? insID)
        {
            //if (!insID.HasValue)
            //    insID = 1;
            string UN = User.Identity.Name.Split('\\').Last();

            Client client = db.Clients.Find(ID);
            ViewBag.Client = client.Name;
            ViewBag.ClientID = Convert.ToString(client.ClientID);
            ViewBag.BranchID = new SelectList(db.Branches.Where(I => I.Active), "BranchID", "BranchName");
            ViewBag.CurrencyTo = new SelectList(db.Currencies.Where(I => I.Active), "CurrencyID", "CurrencyName");
            ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes.Where(I => I.Active && !I.Name.Equals("--")), "InstructionTypeID", "Name");
            ViewBag.InstructionType = db.InstructionTypes.Where(I => I.Active).First();
            return View();
            
        }
        
        // POST: Instructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructionTypeID,Amount,ClientID,StatusID,CurrencyTo,BranchID,EERef")] Instruction instruction, string clientID, string comment, string statusID)
        {
            string UN = User.Identity.Name.Split('\\').Last();
            if (String.IsNullOrEmpty(statusID))
            {
                try
                {
                    Client client = db.Clients.Find(Convert.ToInt32(clientID));
                    ViewBag.Client = client.Name;
                    ViewBag.ClientID = Convert.ToString(client.ClientID);

                    ViewBag.BranchID = new SelectList(db.Branches.Where(I => I.Active), "BranchID", "BranchName");
                    ViewBag.Branch = client.BranchID;

                    ViewBag.CurrencyTo = new SelectList(db.Currencies.Where(I => I.Active), "CurrencyID", "CurrencyName");

                    ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes.Where(I => I.Active), "InstructionTypeID", "Name");
                    ViewBag.InstructionType = db.InstructionTypes.Where(I => I.InstructionTypeID == instruction.InstructionTypeID).First();

                    InstructionTypesController InsT = new InstructionTypesController();
                    List<Status> Statuses = InsT.StatusLooper(instruction.InstructionTypeID);
                    ViewBag.StatusID = new SelectList(Statuses, "StatusID", "Name");
                    ViewBag.Status = Statuses.ElementAt(2);

                    return View();
                }
                catch(Exception)
                {
                    return View();
                }
            }
            else if(instruction.InstructionTypeID == 1)
            {
                Client client = db.Clients.Find(Convert.ToInt32(clientID));
                ViewBag.Client = client.Name;
                ViewBag.ClientID = Convert.ToString(client.ClientID);

                ViewBag.BranchID = new SelectList(db.Branches.Where(I => I.Active), "BranchID", "BranchName");
                ViewBag.Branch = client.BranchID;

                ViewBag.CurrencyTo = new SelectList(db.Currencies.Where(I => I.Active), "CurrencyID", "CurrencyName");

                ViewBag.InstructionTypeID = new SelectList(db.InstructionTypes.Where(I => I.Active), "InstructionTypeID", "Name");
                ViewBag.InstructionType = db.InstructionTypes.Where(I => I.InstructionTypeID == instruction.InstructionTypeID).First();

                return View();
            }
            else
            {
                instruction.ClientID = Convert.ToInt32(clientID);
                instruction.CreateUser = db.Users.Where(U => U.Username.Equals(UN, StringComparison.InvariantCultureIgnoreCase)).First().UserID;
                instruction.FromUser = db.Users.Where(U => U.Username == UN).First().UserID;
                instruction.CurrencyFrom = db.Currencies.First().CurrencyID;
                instruction.CreateDate = DateTime.Now;
                instruction.ToUser = 1;

                db.Instructions.Add(instruction);
                db.SaveChanges();

                Models.Action action = new Models.Action();
                action.ActionDate = DateTime.Now;
                action.Comment = comment;
                action.InstructionID = db.Instructions.ToList().Last().InstructionID;
                action.StatusID = instruction.StatusID;
                action.UserID = db.Users.Where(U => U.Username == UN).First().UserID;
                db.Actions.Add(action);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
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
            ViewBag.Branch = instruction.Branch.BranchName;
            ViewBag.Client = instruction.Client.AccountNumber + " - " + instruction.Client.Name;
            ViewBag.CurrencyFrom = instruction.Currency.CurrencyName;
            ViewBag.CurrencyTo = instruction.Currency1.CurrencyName;
            ViewBag.InstructionType = instruction.InstructionType.Name;
            ViewBag.FromUser = instruction.User.Name + " " + instruction.User.Surname;
            ViewBag.Commentary = instruction.User.Name + " " + instruction.User.Surname;
            ViewBag.EERef = instruction.EERef;
            List<Models.Action> Actions = db.Actions.Where(A => A.InstructionID == id).ToList();

            return View(Actions);
        }

        // POST: Instructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string comment, string EERef)
        {

            return View();
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

        public List<Status> StatusLooper (int ID)
        {
            InstructionTypesController Ins = new InstructionTypesController();
            List<Status> S = Ins.StatusLooper(ID);
            Status status = db.Status.Find(ID);
            foreach(Status s in S)
            {
                if(s.StatusID == status.StatusID)
                {
                    S.Remove(status);
                    S.Insert(0, status);
                }
            }
            return S;
        }

        public ActionResult GetNext()
        {
            string UN = User.Identity.Name.Split('\\').Last();
            User U = db.Users.Where(Us => Us.Username.Equals(UN, StringComparison.InvariantCultureIgnoreCase)).First();
            List<UserRole> UR = db.UserRoles.Where(R => R.UserID == U.UserID).ToList();
            List<Role> Roles = new List<Role>();
            foreach (UserRole UserRole in UR)
            {
                Roles.Add(UserRole.Role);
            }
            List <Status> Statuses = new List<Status>();
            foreach(Role Role in Roles)
            {
                foreach(Status S in Role.Status)
                {
                    Statuses.Add(S);
                }
            }
            List<Instruction> instructions = new List<Instruction>();
            foreach(Status S in Statuses)
            {
                List<Instruction> ins = db.Instructions.Where(I => I.ToUser == 1 && I.StatusID == S.StatusID || I.ToUser == U.UserID && I.StatusID == S.StatusID).ToList();
                instructions.AddRange(ins);
            }
            return View(instructions);
        }
       
        public ActionResult Comment(int? id)
        {
            Instruction instruction = db.Instructions.Find(id);
            ViewBag.Instruction = id;
            ViewBag.Branch = instruction.Branch.BranchName;
            ViewBag.Client = instruction.Client.AccountNumber + " - " + instruction.Client.Name;
            ViewBag.CurrencyFrom = instruction.Currency.CurrencyName;
            ViewBag.CurrencyTo = instruction.Currency1.CurrencyName;
            ViewBag.InstructionType = instruction.InstructionType.Name;
            ViewBag.FromUser = instruction.User.Name + " " + instruction.User.Surname;
            InstructionTypesController InsT = new InstructionTypesController();
            List<Status> Statuses = InsT.StatusLooper(instruction.InstructionTypeID);
            ViewBag.StatusID = new SelectList(Statuses, "StatusID", "Name");
            ViewBag.Status = Statuses.ElementAt(2);

            List<Models.Action> action = db.Actions.Where(A => A.InstructionID == id).ToList();
            ViewBag.Actions = action;
            ViewBag.EERef = instruction.EERef;
            return View(action);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(string comment, string EERef, int StatusID, int InstructionID)
        {
            Models.Action action = new Models.Action();

            action.ActionDate = DateTime.Now;
            action.Comment = comment;

            Instruction ins = db.Instructions.Where(I => I.InstructionID == InstructionID).FirstOrDefault();

            if (!ins.EERef.Equals(EERef))
            {
                ins.EERef = EERef;
            }
            ins.StatusID = StatusID;
            db.Entry(ins).State = EntityState.Modified;


            action.InstructionID = ins.InstructionID;

            Status S = db.Status.Find(StatusID);
            action.StatusID = S.StatusID;

            string UN = User.Identity.Name.Split('\\').Last();
            action.UserID = db.Users.Where(U => U.Username == UN).First().UserID;

            db.Actions.Add(action);
            db.SaveChanges();

            return RedirectToAction("GetNext");
        }

    }
}
 