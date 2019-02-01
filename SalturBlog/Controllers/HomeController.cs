using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class HomeController : Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        public ActionResult Index()
        {
            ViewBag.Url = System.Web.HttpContext.Current.Request.ApplicationPath;
            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article.AsEnumerable()                            
                            where (article.ArticleStatus == true)
                            select new ArticleModel
                            {
                                Id = article.Id,
                                ArticleTitle = article.ArticleTitle,
                                ArticleAuthor = article.ArticleAuthor,
                                ArticleContent = article.ArticleContent,
                                ArticLinkUrl = article.ArticleUrl,
                                ArticleDate = article.ArticleDate,
                                ArticleReading = article.ArticleReading.Value,
                                ArticleTags = article.ArticleTags,
                                ArticleCatagory = article.ArticleCatagory,
                                HomeArticleImageUrl = article.HomeImageUrl

                            }).OrderByDescending(m=>m.ArticleCatagory == Constants.Catagory.Seyahat).ToList();
            ViewBag.Keywords = "Seyahat, Alışveriş,Eğlence,Saltur Blog, Saltur Turizm";
            ViewBag.Title = "Saltur Blog | 2017";
            ViewBag.Description = "1980 yılında Ankara’da kurulan Saltur Turizm bu alanda pek cok ilke imza atmıştır.";
            var authorList = ArticleModel.GroupBy(u => new { u.ArticleAuthor}).Select(grp => grp.FirstOrDefault()).ToList();
            ViewBag.Authors = authorList.ToList();
            return View(ArticleModel);
        }

        public ActionResult Advertisement()
        {
            return View();
        }

        public ActionResult Authors()
        {
            if (System.Web.HttpContext.Current.Request.Url.AbsolutePath.Contains("yazarlar"))
            {
                ViewBag.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath.Substring(1,8);

            }

            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article.AsEnumerable()
                            where (article.ArticleStatus == true)
                            select new ArticleModel
                            {
                                Id = article.Id,
                                ArticleTitle = article.ArticleTitle,
                                ArticleAuthor = article.ArticleAuthor,
                                ArticleContent = article.ArticleContent,
                                ArticLinkUrl = article.ArticleUrl,
                                ArticleDate = article.ArticleDate,
                                ArticleReading = article.ArticleReading.Value,
                                ArticleTags = article.ArticleTags,
                                ArticleCatagory = article.ArticleCatagory,
                                HomeArticleImageUrl = article.HomeImageUrl

                            }).ToList();

            var authorList = ArticleModel.GroupBy(u => new { u.ArticleAuthor }).Select(grp => grp.FirstOrDefault()).ToList();
            ViewBag.Authors = authorList.ToList();
            return View();
        }

        public ActionResult TopArticle()
        {
            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article.AsEnumerable()
                            where (article.ArticleStatus == true)
                            select new ArticleModel
                            {
                                Id = article.Id,
                                ArticleTitle = article.ArticleTitle,
                                ArticleAuthor = article.ArticleAuthor,
                                ArticleContent = article.ArticleContent,
                                ArticleDate = article.ArticleDate,
                                ArticLinkUrl = article.ArticleUrl,
                                ArticleReading = Convert.ToInt32(article.ArticleReading),
                                ArticleTags = article.ArticleTags,
                                ArticleCatagory = article.ArticleCatagory,
                                HomeArticleImageUrl = article.HomeImageUrl

                            }).Take(3).OrderByDescending(m => m.ArticleReading).ToList();
            return View(ArticleModel);
        }

        public ActionResult Gallery()
        {

            return View(db.Gallery.ToList());
        }

        public ActionResult ArticleDetail(string articleLinkUrl)
        {
          
            if (articleLinkUrl == null)
            {
                return RedirectToAction("", "");
            }
                ArticleModel articlemodel = (from article in db.Article.AsEnumerable()
                                         join author in db.Author on article.ArticleAuthor equals author.AuthorName
                                         where article.ArticleStatus == true && article.ArticleUrl == articleLinkUrl

                                         select new ArticleModel
                                         {
                                             Id = article.Id,
                                             ArticleTitle = article.ArticleTitle,
                                             ArticleAuthor = article.ArticleAuthor,
                                             ArticleContent = article.ArticleContent,
                                             ArticLinkUrl = article.ArticleUrl,
                                             ArticleDate = article.ArticleDate,
                                             ArticleReading = article.ArticleReading.Value,
                                             ArticleTags = article.ArticleTags,
                                             ArticleCatagory = article.ArticleCatagory,
                                             HomeArticleImageUrl = article.HomeImageUrl,
                                             AuthorName = author.AuthorName,
                                             AuthorAbout = author.AuthorAbout,
                                             AuthorFacebook = author.AuthorFacebook,
                                             AuthorImageUrl = author.AuthorImageUrl,
                                             AuthorTwitter = author.AuthorTwitter,
                                             AuthorInstagram = author.AuthorInstagram,
                                             Tags = article.ArticleTags.Split(',')

                                         }).FirstOrDefault();
            if (articlemodel != null)
            {
                ViewBag.Keywords = articlemodel.ArticleTags;
                ViewBag.Title = articlemodel.ArticleTitle;
                ViewBag.Title = "Saltur Blog | " + articlemodel.ArticleTitle;

                if (ViewBag.Title == Constants.Catagory.Seyahat)
                {
                    ViewBag.Description = "Seyahat bir tutku olur zamanla. Gezmek olgunlaştırır insanı. Sizi farkında olmadan vizyon ve bakış açısınızı genişletir. ";
                }
                else if (ViewBag.Title == Constants.Catagory.Alisveris)
                {
                    ViewBag.Description = "Stres atmanın ve ekonomik olarak serbest kalmanın cüzdanlara yapılan en güzel darbedir alışveriş yapmak. Bazı darbeler mutlu son ile başlar ve biter.";
                }
                else
                {
                    ViewBag.Description = "Eğlenmek icin yollar sizi bekliyor. Yeni insanlar tanımak, yeni yerler görmek ve yeni maceralara atılmak istiyorsanız durmayın. ";
                }
            }
           
            ViewBag.Url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            try
            {
                List<ArticleModel> relatedPost;
                relatedPost = (from article in db.Article.AsEnumerable()
                               where (article.ArticleStatus == true && articlemodel.ArticleCatagory == article.ArticleCatagory)
                               select new ArticleModel
                               {
                                   Id = article.Id,
                                   ArticleAuthor = article.ArticleAuthor,
                                   ArticleTitle = article.ArticleTitle,
                                   ArticLinkUrl = article.ArticleUrl,
                                   ArticleDate = article.ArticleDate,
                                   ArticleReading = article.ArticleReading.Value,
                                   ArticleCatagory = article.ArticleCatagory,
                                   HomeArticleImageUrl = article.HomeImageUrl

                               }).Take(3).ToList();
                ViewBag.RelatedPost = relatedPost;

                var authorList = relatedPost.GroupBy(u => new { u.ArticleAuthor }).Select(grp => grp.FirstOrDefault()).ToList();
                ViewBag.Authors = authorList.ToList();
                if (Session["ArticleReading" + articlemodel.Id.ToString()] == null)
                {
                    var mevcut = db.Article.Find(articlemodel.Id);
                    mevcut.ArticleReading += 1;
                    Session["ArticleReading" + articlemodel.Id.ToString()] = true;
                    db.SaveChanges();
                }
                return View(articlemodel);
            }
            catch (Exception)
            {
                
                return RedirectToAction("", "");
            }
          
        }

    }
}