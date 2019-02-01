using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class CatagoryController : Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        public ActionResult Index(string catagoryName)
        {
            //ViewBag.Authors = db.Author.ToList();
            ViewBag.Url = catagoryName;

            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article.AsEnumerable()                          
                            where (article.ArticleStatus == true && Utils.UrlDuzenleme.UrlCevir(article.ArticleCatagory).ToLower() == catagoryName)
                            select new ArticleModel
                            {
                                Id = article.Id,
                                ArticleTitle = article.ArticleTitle,
                                ArticleAuthor = article.ArticleAuthor,
                                ArticleContent = article.ArticleContent,
                                ArticleDate = article.ArticleDate,
                                ArticLinkUrl = article.ArticleUrl,
                                ArticleReading = article.ArticleReading.Value,
                                ArticleTags = article.ArticleTags,
                                ArticleCatagory = article.ArticleCatagory,
                                HomeArticleImageUrl = article.HomeImageUrl

                            }).OrderByDescending(m => m.Id).ToList();

            ViewBag.Keywords = ArticleModel.FirstOrDefault().ArticleTags;
         
            ViewBag.Title = "Saltur Blog | " +  ArticleModel.FirstOrDefault().ArticleCatagory;

          
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

            if (ArticleModel.Count != 0)
            {
                ViewBag.CategoryName = ArticleModel.FirstOrDefault().ArticleCatagory;
            }
            else
            {
                return RedirectToAction("", "");
            }
        
            return View(ArticleModel);
        }

       
    }
}