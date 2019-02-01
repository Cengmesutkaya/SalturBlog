using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SalturBlog.BlogService
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class BlogServices : System.Web.Services.WebService
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();

        public class BlogItemList
        {
            public int Id { get; set; }
            public string MakaleCatagory { get; set; }
            public string MakaleTitle { get; set; }
            public string TitleLink { get; set; }
            public string MakaleContent { get; set; }
            public string MakaleResim { get; set; }
            public string StatusProcess { get; set; }

        }
    
        [WebMethod]
        public List<BlogItemList> GetArticle(string password)
        {
            var apikontrol = db.Author.FirstOrDefault(m => m.AuthorPassword == password && m.AuthorName == Constants.AdminRole.AdminName);

            if (apikontrol != null)
            {
                var makalelistesi = (from m in db.Article

                                  select new BlogItemList
                                  {
                                      Id = m.Id,
                                      MakaleCatagory = m.ArticleCatagory,
                                      MakaleContent = m.ArticleContent,
                                      MakaleTitle = m.ArticleTitle,
                                      TitleLink = m.ArticleUrl,
                                      MakaleResim = m.HomeImageUrl,

                                  }).Take(4).ToList();

                return makalelistesi;
            }
            else
            {
                List<BlogItemList> shata = new List<BlogItemList>();
                BlogItemList SH = new BlogItemList();
                SH.StatusProcess = "0-Api için yetkisiz kullanıcı !";
                shata.Add(SH);
                return shata;
            }
        }

}
}
