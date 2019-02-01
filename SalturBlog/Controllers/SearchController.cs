using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class SearchController : Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        [HttpPost]
        public ActionResult Index(string searchText)
        {
            List<ArticleModel> ArticleModel;
            ArticleModel = (from article in db.Article
                            where article.ArticleCatagory.Contains(searchText) || article.ArticleAuthor.Contains(searchText)
                            || article.ArticleTitle.Contains(searchText) || article.ArticleTags.Contains(searchText)

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
            if (ArticleModel.Count() != 0)
            {
            ViewBag.Keywords = ArticleModel.FirstOrDefault().ArticleTags;
            ViewBag.Title = "Saltur Blog | " + ArticleModel.FirstOrDefault().ArticleCatagory;


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
            if (ArticleModel.Count() != 0)
            {
                return View(ArticleModel);

            }
            else
            {
                ViewBag.Result = "VeriYok";
                return View(ArticleModel);

            }
        }

        public ActionResult RelatedSearchPost()
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

            ViewBag.RelatedPost = ArticleModel;
            return View();
        }
    }
}