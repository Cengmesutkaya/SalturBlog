using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class AuthorController : Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        public ActionResult Index(string authorName)
        {
             if (authorName == null)
            {
                return RedirectToAction("","");
            }
            ArticleAuthor(authorName);
            ViewBag.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath.Substring(1,8).ToString();

            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article.AsEnumerable()
                            join author in db.Author on article.ArticleAuthor equals author.AuthorName
                            where (article.ArticleStatus == true && Utils.UrlDuzenleme.UrlCevir(article.ArticleAuthor).ToLower() == authorName)

                            select new ArticleModel
                            {
                                Id = article.Id,
                                ArticleTitle = article.ArticleTitle,
                                ArticleAuthor = article.ArticleAuthor,
                                ArticleContent = article.ArticleContent,
                                ArticleDate = article.ArticleDate,
                                ArticleReading = Convert.ToInt32(article.ArticleReading),
                                ArticleTags = article.ArticleTags,
                                ArticLinkUrl = article.ArticleUrl,
                                ArticleCatagory = article.ArticleCatagory,
                                HomeArticleImageUrl = article.HomeImageUrl

                            }).OrderByDescending(m=>m.Id).ToList();
            ViewBag.Keywords = ArticleModel.FirstOrDefault().ArticleTags;
            ViewBag.Title = "Yazar | " + ArticleModel.FirstOrDefault().ArticleAuthor;
          
           
            if (ArticleModel.FirstOrDefault().ArticleCatagory == Constants.Catagory.Seyahat)
            {
                ViewBag.Description = "Seyahat bir tutku olur zamanla. Gezmek olgunlaştırır insanı. Sizi farkında olmadan vizyon ve bakış açısınızı genişletir. ";
            }
            else if (ArticleModel.FirstOrDefault().ArticleCatagory == Constants.Catagory.Alisveris)
            {
                ViewBag.Description = "Stres atmanın ve ekonomik olarak serbest kalmanın cüzdanlara yapılan en güzel darbedir alışveriş yapmak. Bazı darbeler mutlu son ile başlar ve biter.";
            }
            else
            {
                ViewBag.Description = "Eğlenmek icin yollar sizi bekliyor. Yeni insanlar tanımak, yeni yerler görmek ve yeni maceralara atılmak istiyorsanız durmayın. ";
            }
            if (ArticleModel.Count() != 0)
            {
                ViewBag.AuthorName = ArticleModel.FirstOrDefault().ArticleAuthor;
            }
            else
            {
                return RedirectToAction("", "");
            }
          
            return View(ArticleModel);
        }
        
        public ActionResult ArticleAuthor(string authorName)
        {
            AuthorModel AuthorModel = (from author in db.Author.AsEnumerable()
                                         where Utils.UrlDuzenleme.UrlCevir(author.AuthorName).ToLower() == authorName
                                         select new AuthorModel
                                         {
                                             AuthorName = author.AuthorName,
                                             AuthorAbout = author.AuthorAbout,
                                             AuthorImageUrl = author.AuthorImageUrl,
                                             AuthorFacebook = author.AuthorFacebook,
                                             AuthorInstagram = author.AuthorInstagram,
                                             AuthorTwitter = author.AuthorTwitter,
                                         }).FirstOrDefault();
            return View(AuthorModel);
        }

    }
}