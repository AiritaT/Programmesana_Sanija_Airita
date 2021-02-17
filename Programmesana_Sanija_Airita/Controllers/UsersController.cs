using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Programmesana_Sanija_Airita.Controllers.DataAccess;
using Programmesana_Sanija_Airita.Models;
using System.IO;
using System.Web.Security;

namespace Programmesana_Sanija_Airita.Controllers
{
    public class UsersController : Controller
    {
        private ProgrammesanaEntities db = new ProgrammesanaEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User u, string ConfirmPassword)
        {
            try {

                if (ModelState.IsValid)
                {
                    if (ConfirmPassword == u.Password)
                    {
                        UsersRepository ur = new UsersRepository();

                        if (ur.DoesUsernameExist(u.Username) == true)
                        {
                            ViewBag.Error = "Username already exist.";
                        }
                        else
                        {
                            u.Password = u.Password;
                            ur.AddUser(u);
                            Role defaultrole = ur.GetDefaultRole();
                            ur.AllocateRoleToUser(u, defaultrole);

                            ViewBag.Message = "Registration successful";
                            ModelState.Clear();
                        }

                    }
                    else
                    {
                        ViewBag.Error = "password do not match";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Registartion Failed";
            }
            return View(u);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login (string username, string password)
        {

            UsersRepository ur = new UsersRepository();
            User trylogin = ur.GetUserByUsername(username);

            if(trylogin.Blocked == true)
            {

                ViewBag.Error = "Your account is blocked. Contact Administrator";
            }
            else
            {
                if (ur.Login(username, password) == true)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(username, true);
                    return RedirectToAction("Files", "Files");
                }
                else
                {
                    ViewBag.Error = "Login failed";
                }
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult List()
        {
            UsersRepository ur = new UsersRepository();
            var listOfUser = ur.GetUsers();
            return View(listOfUser);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string username)
        {
            using (ProgrammesanaEntities dc = new ProgrammesanaEntities())
            {
                return View(dc.Users.Where(x => x.Username == username).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(User u)
        {
            try
            {
                using (ProgrammesanaEntities dc = new ProgrammesanaEntities())
                {
                    var myUser = dc.Users.SingleOrDefault(x => x.Username == u.Username);

                    myUser.Blocked = u.Blocked;
                    dc.SaveChanges();

                }
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }
    }
}
