using SalturBlog.Models;
using SalturBlog.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class AdminlerController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.Author.ToList());
        }

        public ActionResult Create(Author author, HttpPostedFileBase ImageFiles)
        {
            ViewBag.AdminRol = new SelectList(
          new List<SelectListItem>
          {
             new SelectListItem { Text = Constants.AdminRole.Admin, Value = Constants.AdminRole.Admin},
             new SelectListItem { Text = Constants.AdminRole.User, Value = Constants.AdminRole.User},
          }, "Value", "Text");
     
            if (author.AuthorEmail == null && author.AuthorAbout == null)
            {
                ViewBag.Visibility = "hidden";
                return View();
            }
            else
            {
                var emailKontrol = db.Author.Where(m => m.AuthorEmail == author.AuthorEmail).FirstOrDefault();
                if (emailKontrol == null)
                {
                    var md5pass = Crypto.Hash(author.AuthorPassword.Trim(), "MD5").ToLower();
                    author.AuthorPassword = md5pass;
                    author.AuthorTwitter = "https://twitter.com/";
                    if (ImageFiles != null)
                    {
                        FileInfo fileinfo = new FileInfo(ImageFiles.FileName);
                        WebImage img = new WebImage(ImageFiles.InputStream);
                        string uzanti = (author.AuthorEmail+ fileinfo.Extension).ToLower();
                        //img.Resize(400, 225, false, true);
                        //img.Crop(1, 1);
                        string TamYolYeri = "~/Content/img/autor/" + uzanti;
                        img.Save(Server.MapPath(TamYolYeri));
                        author.AuthorImageUrl = "/Content/img/autor/" + uzanti;
                    }
                    db.Author.Add(author);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Visibility = "visible";
                    return View();
                }
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author Author = db.Author.Find(id);
            if (Author == null)
            {
                return HttpNotFound();
            }
            if (Author.UserRol == Constants.AdminRole.Admin)
            {
                ViewBag.AdminRol = new SelectList(
             new List<SelectListItem>
            {
             new SelectListItem { Text = Constants.AdminRole.Admin, Value = Constants.AdminRole.Admin},
             new SelectListItem { Text = Constants.AdminRole.User, Value = Constants.AdminRole.User},
                  }, "Value", "Text");
            }
            else
            {
                ViewBag.AdminRol = new SelectList(
                new List<SelectListItem>
                {
             new SelectListItem { Text = Constants.AdminRole.User, Value = Constants.AdminRole.User},
             new SelectListItem { Text = Constants.AdminRole.Admin, Value = Constants.AdminRole.Admin},
                     }, "Value", "Text");
            }

            ViewBag.PassWord = Author.AuthorPassword;
            return View(Author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Author author, HttpPostedFileBase ImageFiles)
        {
            int SifreUzunluk = 32;

            if (ModelState.IsValid)
            {
                if (author.AuthorPassword.Length != SifreUzunluk)
                {
                    var md5pass = Crypto.Hash(author.AuthorPassword.Trim(), "MD5").ToLower();
                    author.AuthorPassword = md5pass;
                }

                db.Entry(author).State = EntityState.Modified;
              


                if (ImageFiles != null)
                {
                    FileInfo fileinfo = new FileInfo(ImageFiles.FileName);
                    WebImage img = new WebImage(ImageFiles.InputStream);
                    string uzanti = (author.AuthorEmail + fileinfo.Extension).ToLower();
                    //img.Resize(400, 225, false, true);
                    //img.Crop(1, 1);
                    string TamYolYeri = "~/Content/img/autor/" + uzanti;

                    if (System.IO.File.Exists(Server.MapPath(TamYolYeri)))
                    {
                        System.IO.File.Delete(Server.MapPath(TamYolYeri));
                        img.Save(Server.MapPath(TamYolYeri));
                    }
                    else
                    {
                        img.Save(Server.MapPath(TamYolYeri));

                    }
                    author.AuthorImageUrl = "/Content/img/autor/" + uzanti;
                }
                else
                {
                    db.Entry(author).Property(x => x.AuthorImageUrl).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        public ActionResult Delete(int id)
        {
            Author Author = db.Author.Find(id);
            db.Author.Remove(Author);
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