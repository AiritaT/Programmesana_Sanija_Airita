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
using Programmesana_Sanija_Airita.HashPassword;
using System.Web.Security;

namespace Programmesana_Sanija_Airita.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User u, string ConfirmPassword)
        { 
            try {

                if (ModelState.IsValid)
                {
                    if (ConfirmPassword == u.Password)
                    {
                        UsersRepository ur = new UsersRepository();
                        if (ur.DoesUsernameExist(u.Username) == true)
                        {
                            ViewBag.Error = "Username already exist";
                            //return View(u);
                        }
                        else
                        {
                            u.Password = Encryption.HashPassword(u.Password)
                                ;
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
                else
                {
                    ViewBag.Error = "Check your inputs";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Registration failed!";
            }
            return View();
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

            if (trylogin == null)
            {
                ViewBag.Error = "There is no account";
            }
               else if (trylogin.Blocked == true)
                {

                    ViewBag.Error = "Your account is blocked. Contact Administrator";
                }
                else if (ur.Login(username, Encryption.HashPassword(password)) == true)
                {

                    System.Web.Security.FormsAuthentication.SetAuthCookie(username, true);
                    return RedirectToAction("Files", "Files");
                }
                else if (ur.Login(username, Encryption.HashPassword(password)) == false)
                {
                    ViewBag.Error = "Login failed";
                }
                else
                {
                    ViewBag.Error = "Login failed";
                }
            
            
            return View();
                 
            }
       // [Authorize]
       // [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin")]
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
            using (ProgrammesanaEntities1 dc = new ProgrammesanaEntities1())
            {
                return View(dc.Users.Where(x => x.Username == username).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(User u)
        {
            try
            {
                using (ProgrammesanaEntities1 dc = new ProgrammesanaEntities1())
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
        [HttpGet]
        public ActionResult UserProfile(User u)
        {
            /*using (ProgrammesanaEntities1 dc = new ProgrammesanaEntities1())
            {
                return View(dc.Users.Where(x => x.Username == username).FirstOrDefault());
            }*/
            try{
                UsersRepository ir = new UsersRepository();
                var myUser = ir.GetUsers().SingleOrDefault(x => x.Username == u.Username);
                return View(myUser);
            }
            catch
            {
                return RedirectToAction("List");
            }
        }
        public ActionResult EditProfile(string username)
        {
            using (ProgrammesanaEntities1 dc = new ProgrammesanaEntities1())
            {
                return View(dc.Users.Where(x => x.Username == username).FirstOrDefault());
            }
        }
        public ActionResult SaveProfile(User u)
        {
            ProgrammesanaEntities1 db = new ProgrammesanaEntities1();
            User user = db.Users.Where(x => x.Username == u.Username).FirstOrDefault();

            u.Name = u.Name;
            u.Surname = u.Surname;
            u.Username = u.Username;
            u.Password = u.Password;
            db.SaveChanges();

            db.Dispose();
            return Redirect("Users");
        }
        /*[HttpGet]
        public ActionResult Share(string username)
        {
           /* using (ProgrammesanaEntities1 db = new ProgrammesanaEntities1())
            {
                return View(db.Users.Where(x => x.Username == username).FirstOrDefault());
            }
            /*File f = new File();

             using(ProgrammesanaEntities1 db = new ProgrammesanaEntities1())
             {

                 f.UserCollection = db.Users.ToList();
             }

             return View(it);*/
    }
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Share (File f, Guid id)
    {

        FilesRepository ir = new FilesRepository();

        var file = ir.GetFiles().SingleOrDefault(x => x.id == id);

        file.Share = f.Share;

        ir.Entity.SaveChanges();

        return RedirectToAction("List", "Files");
    }*/
    

}
