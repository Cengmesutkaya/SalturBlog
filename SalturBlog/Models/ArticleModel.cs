using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalturBlog.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string ArticleDate { get; set; }

        public int ArticleReading { get; set; }

        public string ArticleTags { get; set; }


        public string ArticleCatagory { get; set; }

        public string ArticleAuthor { get; set; }

        public string ArticLinkUrl { get; set; }

        public List<Author> AuthorList { get; set; }

        public string ArticleStatus { get; set; }

        public string HomeArticleImageUrl { get; set; }
        
        public string AuthorName { get; set; }

        public string AuthorAbout { get; set; }

        public string AuthorImageUrl { get; set; }

        public string AuthorFacebook { get; set; }

        public string AuthorTwitter { get; set; }

        public string AuthorInstagram { get; set; }

        public string[] Tags { get; set; }

    }
}