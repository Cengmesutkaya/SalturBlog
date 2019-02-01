using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class LoginController : Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        public ActionResult Index(Author author)
        {
            if (author.AuthorEmail == null)
            {
                ViewBag.Visibility = "hidden";
                return View();
            }
            else
            {
                var md5password = Crypto.Hash(author.AuthorPassword, "MD5").ToLower();
                var admin = db.Author.Where(m => m.AuthorEmail == author.AuthorEmail && m.AuthorPassword == md5password).FirstOrDefault();

                if (admin != null)
                {
                    Session["UserName"] = admin.AuthorName;
                    Session["UserImageUrl"] = admin.AuthorImageUrl;

                    Session["AdminRole"] = admin.UserRol;
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ViewBag.Visibility = "visible";
                    return View();
                }
            }

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}