﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Programmesana_Sanija_Airita.Controllers.DataAccess;
using Programmesana_Sanija_Airita.Models;
using File = Programmesana_Sanija_Airita.Models.File;

namespace Programmesana_Sanija_Airita.Controllers
{
    public class FilesController : Controller
    {
        public ActionResult Files()
        {
            FilesRepository fr = new FilesRepository();
            var listOfFiles = fr.GetFiles();
            return View(listOfFiles);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create(File data)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    FilesRepository fr = new FilesRepository();
                    UsersRepository ur = new UsersRepository();
                    var FileCreater = ur.GetUserByUsername(User.Identity.Name);

                    data.User_id = FileCreater.Username;
                    fr.AddFiles(data);


                    //new LogsRepository().Log(User.Identity.Name, controllerName + "\\" + actionName, "Post" + data.Id + "added", LogType.Information);
                    ViewBag.Message = "Item added successfully";

                    return RedirectToAction("Upload", new { id = data.id });
                }
                else
                {
                    ViewBag.Error = "Check your inputs";
                }
            }
            catch (Exception ex)
            {
                //new LogsRepository().Log(User.Identity.Name, controllerName + "\\" + actionName, ex, LogType.Error);
                ViewBag.Error = "Item failed to be added";
            }

            return View();
        }
       // [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Search(string keyword)
        {
            FilesRepository ir = new FilesRepository();
            var filteredList = ir.GetFiles().Where(File => File.Title.Contains(keyword));

            return View("Files", filteredList);
            //metids list and search
        }

        /*[ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            FilesRepository fr = new FilesRepository();

              var file = fr.GetFiles().SingleOrDefault(x => x.id == id);
              if (file != null)
                  fr.DeleteFile(file);

              TempData["Message"] = "Deleted successfully";
              return RedirectToAction("Files");
        }*/
        [HttpGet]
        public ActionResult Upload(Guid id)
        {
            //shows the page
            FilesRepository fr = new FilesRepository();
            var file = fr.GetFiles().SingleOrDefault(x => x.id == id);
            return View(file);

        }
        [HttpPost]
        public ActionResult Upload(Guid id , HttpPostedFileBase file)
        {
            FilesRepository fr = new FilesRepository();
            if (file != null)
            {
                byte[] readBytes = new byte[2];
                file.InputStream.Read(readBytes, 0, 2);
                file.InputStream.Position = 0;

                /*if (
                    (readBytes[0] == 80 & readBytes[1] == 75) || //zip
                    (readBytes[0] == 82 & readBytes[1] == 97) || //rar
                    (readBytes[0] == 137 & readBytes[1] == 80) || //png
                    (readBytes[0] == 105 & readBytes[1] == 115)//mp4
                    )

                {*/
                    if (file.ContentLength <= 104857600)
                    {
                        //string Path = Server.MapPath("//FileStorage") + "//" + newFilename;
                        string name = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/FileStorage") + "//" + name);
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded";
                        Upload u = new Upload();
                        u.File_id = id;
                        u.Path = name;
                        fr.AddUpload(u);
                        ViewBag.Message = "File is uploaded successfully";
                    }
                    else
                        ViewBag.Error = "File should be smaller than 2 GB";
                /*}
                else ViewBag.Error = "File type not allowed";*/
            }
            var fails = fr.GetFiles().SingleOrDefault(x => x.id == id);
            return View(fails);

        }
        
        //[ValidateAntiForgeryToken]
        public ActionResult Download(string id)
        {

            var FilePath = "~/FileStorage/" + id;
            return File(FilePath , "application/force-stream", System.IO.Path.GetFileName(FilePath));

        }
        public ActionResult Details(Guid id)
        {
            try
            {
                FilesRepository ir = new FilesRepository();
                //encryption.decryptquerystring
                var myItem = ir.GetFiles().SingleOrDefault(x => x.id == id);
                return View(myItem);
            }
            catch
            {
                TempData["error"] = "Value is not valid";
                return RedirectToAction("List");
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult Share(string username)
        {
            
            using (ProgrammesanaEntities1 dc = new ProgrammesanaEntities1 ())
            {
                return View(dc.Users.Where(x => x.Username == username).FirstOrDefault());
            }
        }
       [HttpGet]
        public ActionResult Edit (Guid id)
        {
            using (ProgrammesanaEntities1 db = new ProgrammesanaEntities1())
            {
                return View (db.Files.Where(x => x.id == id).FirstOrDefault());
            };
        }
        [HttpPost]
        public ActionResult Edit(File file)
        {
            try
            {
                using (ProgrammesanaEntities1 db = new ProgrammesanaEntities1()){
                      var myFile = db.Files.Where(x => x.id == file.id).FirstOrDefault();

                    myFile.Title = file.Title;
                    myFile.Description = file.Description;
                    myFile.Date = file.Date;
                    myFile.Share = file.Share;
                    myFile.Categories_id = file.Categories_id;
                    db.SaveChanges();
                 }
                return RedirectToAction("Files");
            }
            catch
            {
                return View();
            }

        }
        public ActionResult Delete(Guid id)
        {
            ProgrammesanaEntities1 db = new ProgrammesanaEntities1();
            File file = db.Files.Find(id);
            db.Uploads.RemoveRange(file.Uploads);
            db.Files.Remove(file);
            db.SaveChanges();
            return RedirectToAction("Files");
                
            /*Files.Where(x => x.id == f.id).FirstOrDefault();
            var uploads = db.Uploads.Where(b => b.File_id == e.id).AsEnumerable();
            foreach( var bk in uploads)
            {
                var b = bk;
                e.Uploads.Remove(b);
            }
            db.Files.Remove(e);
            db.SaveChanges();
            db.Dispose();*/
        }
    }

   
}
