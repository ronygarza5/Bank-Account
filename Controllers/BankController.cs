using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BankAccount.Controllers
{
    [Route("bank")]
    public class BankController : Controller
    {
        private HomeContext dbContext;
        public BankController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            User userInDb = dbContext.Users.Include(u => u.MyTransactions).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
            double balance = userInDb.MyTransactions.Sum(a => a.Amount);
            ViewBag.Balance = balance;
            if (userInDb == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.User = userInDb;
            return View();
        }

        [HttpPost("Insert")]
        public IActionResult Insert(TransActions act)
        {
            User userInDb = dbContext.Users.Include(u => u.MyTransactions).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
            double balance = userInDb.MyTransactions.Sum(a => a.Amount);
            if (userInDb == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    double total = balance + act.Amount;
                    if (total < 0)
                    {
                        ModelState.AddModelError("Amount", "You do not have any money in Account.");
                        ViewBag.User = userInDb;
                        ViewBag.Balance = balance;
                        return View("Index");
                    }
                    else
                    {
                        act.UserId = userInDb.UserId;
                        dbContext.TransAction.Add(act);
                        dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Balance = balance;
                    ViewBag.User = userInDb;
                    return View("Index");
                }

            }
        }

    }
}