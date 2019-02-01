using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalturBlog.Models
{
    public class AuthorModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string AuthorAbout { get; set; }

        public string AuthorImageUrl { get; set; }

        public string AuthorFacebook { get; set; }

        public string AuthorTwitter { get; set; }

        public string AuthorInstagram { get; set; }
    }
}