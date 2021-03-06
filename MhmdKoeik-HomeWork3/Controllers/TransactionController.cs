﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MhmdKoeik_HomeWork3.Models;
using MhmdKoeik_HomeWork3.Services;
using Microsoft.AspNet.Identity;

namespace MhmdKoeik_HomeWork3.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private IApplicationDbContext db;

        public TransactionController()
        {
            db = new ApplicationDbContext();
        }

        public TransactionController(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public ActionResult BalanceInquiry()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccount = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First();
            return View(checkingAccount);
        }
        public ActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            var userId = User.Identity.GetUserId();
            var checkingAccount = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First();
            if (ModelState.IsValid)
            {
                transaction.TransactionDate = DateTime.Now;
                transaction.Source = checkingAccount.Name;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult QuickCash(int checkingAccountId, decimal amount)
        {
            var sourceCheckingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var balance = sourceCheckingAccount.Balance;
            if (balance < amount)
            {
                return View("QuickCashInsufficientFunds");
            }
            db.Transactions.Add(new Transaction { CheckingAccountId = checkingAccountId, Amount = -amount, TransactionDate = DateTime.Now, Source = sourceCheckingAccount.Name });
            db.SaveChanges();

            var service = new CheckingAccountService(db);
            service.UpdateBalance(checkingAccountId);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Withdrawal(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
            if (checkingAccount.Balance < transaction.Amount)
            {
                ModelState.AddModelError("Amount", "You have insufficient funds!");
            }

            if (ModelState.IsValid)
            {
                transaction.Amount = -transaction.Amount;
                transaction.TransactionDate = DateTime.Now;
                transaction.Source = checkingAccount.Name;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Transfer(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(Transfer transfer)
        {

            // check for available funds
            var sourceCheckingAccount = db.CheckingAccounts.Find(transfer.CheckingAccountId);
            if (sourceCheckingAccount.Balance < transfer.Amount)
            {
                ModelState.AddModelError("Amount", "You have insufficient funds!");
            }

            // check for a valid destination account
            var destinationCheckingAccount = db.CheckingAccounts.Where(c => c.AccountNumber == transfer.TransactionSource).FirstOrDefault();
            var userId = User.Identity.GetUserId();
            var sourceAccount = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).FirstOrDefault();
            if (destinationCheckingAccount == null)
            {
                ModelState.AddModelError("TransactionSource", "Invalid destination account number.");
            }

            // add debit/credit transactions and update account balances
            if (ModelState.IsValid)
            {
                db.Transactions.Add(new Transaction { CheckingAccountId = transfer.CheckingAccountId, Amount = -transfer.Amount, TransactionDate = DateTime.Now, Source = transfer.TransactionSource });
                db.Transactions.Add(new Transaction { CheckingAccountId = destinationCheckingAccount.Id, Amount = transfer.Amount, TransactionDate = DateTime.Now, Source = sourceAccount.AccountNumber });
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transfer.CheckingAccountId);
                service.UpdateBalance(destinationCheckingAccount.Id);

                return PartialView("_TransferSuccess", transfer);
            }
            return PartialView("_TransferForm");
        }


        public ActionResult AdminTransfer(Transfer transfer)
        {
          
            var acc = db.CheckingAccounts.Where(a => a.AccountNumber == transfer.TransactionSource).FirstOrDefault();
            ViewBag.accounts = db.CheckingAccounts.Distinct().Select(i => new SelectListItem() { Text = i.AccountNumber, Value = i.AccountNumber.ToString() }).ToList();

                return View(acc);
            }


        public ActionResult PrintStatement()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccount = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First();
            return View(checkingAccount.Transactions.ToList());

        }
    }
}