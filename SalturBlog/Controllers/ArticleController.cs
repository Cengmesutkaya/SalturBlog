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
    public class ArticleController : BaseController
    {
        public ActionResult Index()
        {

            List<MakaleModel> makaleModel;
            makaleModel = (from makale in db.Article
                           select new MakaleModel
                           {
                               ArticleAuthor = makale.ArticleAuthor,
                               Id = makale.Id,
                               ArticleCatagory = makale.ArticleCatagory,
                               ArticleDate = makale.ArticleDate,
                               ArticleTitle = makale.ArticleTitle
                           }).ToList();

            return View(makaleModel);
        }


        [ValidateInput(false)]
        public ActionResult Create(Article article, HttpPostedFileBase ImageFiles)
        {

            if (article.ArticleTitle == null && article.ArticleContent == null)
            {
                ViewBag.Visibility = "hidden";
                ViewBag.Kategoriler = new SelectList(
         new List<SelectListItem>
         {
             new SelectListItem { Text = Constants.Catagory.Seyahat, Value = Constants.Catagory.Seyahat},
             new SelectListItem { Text = Constants.Catagory.Alisveris, Value = Constants.Catagory.Alisveris},
             new SelectListItem { Text = Constants.Catagory.Eglence, Value = Constants.Catagory.Eglence},
         }, "Value", "Text");
                return View();
            }
            else
            {
                article.ArticleAuthor = Session["UserName"].ToString();
                article.ArticleDate = DateTime.Now.ToString("MMMM d, yyyy");
                article.ArticleStatus = true;
                article.ArticleReading = 50;
                article.ArticleUrl = UrlDuzenleme.UrlCevir(article.ArticleTitle).ToLower();
                if (ImageFiles != null)
                {
                    FileInfo fileinfo = new FileInfo(ImageFiles.FileName);
                    WebImage img = new WebImage(ImageFiles.InputStream);
                    string uzanti = (article.ArticleUrl + fileinfo.Extension).ToLower();
                    img.Resize(458, 375, false, true);
                    //img.Crop(1, 1);
                    string TamYolYeri = "~/Content/img/blog/" + uzanti;
                    img.Save(Server.MapPath(TamYolYeri));
                    article.HomeImageUrl = "/Content/img/blog/" + uzanti;
                }
                db.Article.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            if (article.ArticleStatus == true)
            {
                ViewBag.ArticleActive = new SelectList(
            new List<SelectListItem>
            {
                                   new SelectListItem { Text = Constants.ArticleActive.ArticleAktif, Value = Constants.ArticleActive.ArticleAktif},
                                   new SelectListItem { Text = Constants.ArticleActive.ArticlePasif, Value = Constants.ArticleActive.ArticlePasif},

            }, "Value", "Text");
            }
            else
            {
                ViewBag.ArticleActive = new SelectList(
                       new List<SelectListItem>
                       {
                          new SelectListItem { Text = Constants.ArticleActive.ArticlePasif, Value = Constants.ArticleActive.ArticlePasif},
                          new SelectListItem { Text = Constants.ArticleActive.ArticleAktif, Value = Constants.ArticleActive.ArticleAktif},

                       }, "Value", "Text");
            }

            if (article.ArticleCatagory == Constants.Catagory.Seyahat)
            {
                ViewBag.Kategoriler = new SelectList(
             new List<SelectListItem>
             {
                       new SelectListItem { Text = Constants.Catagory.Seyahat, Value = Constants.Catagory.Seyahat},
                       new SelectListItem { Text = Constants.Catagory.Alisveris, Value = Constants.Catagory.Alisveris},
                       new SelectListItem { Text = Constants.Catagory.Eglence, Value = Constants.Catagory.Eglence},
             }, "Value", "Text");
            }
            else if (article.ArticleCatagory == Constants.Catagory.Alisveris)
            {
                ViewBag.Kategoriler = new SelectList(
             new List<SelectListItem>
             {
                      new SelectListItem { Text = Constants.Catagory.Alisveris, Value = Constants.Catagory.Alisveris},
                      new SelectListItem { Text = Constants.Catagory.Seyahat, Value = Constants.Catagory.Seyahat},
                      new SelectListItem { Text = Constants.Catagory.Eglence, Value = Constants.Catagory.Eglence},
             }, "Value", "Text");

            }
            else
            {
                ViewBag.Kategoriler = new SelectList(
                    new List<SelectListItem>
                    {
                      new SelectListItem { Text = Constants.Catagory.Eglence, Value = Constants.Catagory.Eglence},
                      new SelectListItem { Text = Constants.Catagory.Alisveris, Value = Constants.Catagory.Alisveris},
                      new SelectListItem { Text = Constants.Catagory.Seyahat, Value = Constants.Catagory.Seyahat},
                    }, "Value", "Text");
            }


            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article article, HttpPostedFileBase ImageFiles,string articleStatus)
        {

            if (article.ArticleTitle != null && article.ArticleContent != null)
            {

                if (articleStatus == Constants.ArticleActive.ArticleAktif)
                {
                    article.ArticleStatus = true;
                }
                else
                {
                    article.ArticleStatus = false;
                }

                db.Entry(article).State = EntityState.Modified;
                db.Entry(article).Property(x => x.ArticleAuthor).IsModified = false;
                db.Entry(article).Property(x => x.ArticleDate).IsModified = false;
                db.Entry(article).Property(x => x.ArticleReading).IsModified = false;

               article.ArticleUrl = UrlDuzenleme.UrlCevir(article.ArticleTitle).ToLower();
                if (ImageFiles != null)
                {
                    FileInfo fileinfo = new FileInfo(ImageFiles.FileName);
                    WebImage img = new WebImage(ImageFiles.InputStream);
                    string uzanti = (article.ArticleUrl + fileinfo.Extension).ToLower();
                    img.Resize(458, 375, false, true);
                    //img.Crop(1, 1);
                    string TamYolYeri = "~/Content/img/blog/" + uzanti;
                    img.Save(Server.MapPath(TamYolYeri));
                    article.HomeImageUrl = "/Content/img/blog/" + uzanti;
                }
                else
                {
                    db.Entry(article).Property(x => x.HomeImageUrl).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult Delete(int id)
        {
            Article article = db.Article.Find(id);
            db.Article.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}