using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MhmdKoeik_HomeWork3.Models;

namespace MhmdKoeik_HomeWork3.Controllers
{
    public class CheckingAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CheckingAccounts
        public ActionResult Index()
        {
            var checkingAccounts = db.CheckingAccounts.Include(c => c.User);
            return View(checkingAccounts.ToList());
        }

        // GET: CheckingAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return View(checkingAccount);
        }

        public ActionResult PrintStatement(int id)
        {
            var checkingAccount = db.CheckingAccounts.Find(id);
            return View(checkingAccount.Transactions.ToList());
        }
        // GET: CheckingAccounts/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Pin");
            return View();
        }

        // POST: CheckingAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountNumber,FirstName,LastName,Balance,ApplicationUserId")] CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                db.CheckingAccounts.Add(checkingAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Pin", checkingAccount.ApplicationUserId);
            return View(checkingAccount);
        }

        // GET: CheckingAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Pin", checkingAccount.ApplicationUserId);
            return View(checkingAccount);
        }

        // POST: CheckingAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountNumber,FirstName,LastName,Balance,ApplicationUserId")] CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Pin", checkingAccount.ApplicationUserId);
            return View(checkingAccount);
        }

        // GET: CheckingAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return View(checkingAccount);
        }

        // POST: CheckingAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            db.CheckingAccounts.Remove(checkingAccount);
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
