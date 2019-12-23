using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {

        private HomeContext dbContext;

        public HomeController(HomeContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User reg)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    reg.Password = Hasher.HashPassword(reg, reg.Password);
                    dbContext.Users.Add(reg);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetString("UserEmail", reg.Email);
                    return RedirectToAction("Index", "Bank");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser login)
        {
            if (ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == login.LoginEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email or Password.");
                    return View("SignIn");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(login, userInDb.Password, login.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email or Password.");
                    return View("SignIn");
                }
                HttpContext.Session.SetString("UserEmail", login.LoginEmail);
                return RedirectToAction("Index", "Bank");
            }
            else
            {
                return View("SignIn");
            }
        }

        [HttpGet("dahboard")]
        public IActionResult DashBoard()
        {
            User userInDb = dbContext.Users.FirstOrDefault( u => u.Email == HttpContext.Session.GetString("UserEmail"));
            if (userInDb == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            return View(userInDb);
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
