using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalturBlog.Models
{
    public class MakaleModel
    {
        public int Id { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleDate { get; set; }

        public string ArticleCatagory { get; set; }

        public string ArticleAuthor { get; set; }
    }
}