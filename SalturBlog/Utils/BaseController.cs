using SalturBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalturBlog.Utils
{
    public class BaseController : System.Web.Mvc.Controller
    {
        public SalturBlog2017Entities db = new SalturBlog2017Entities();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            if (Session["UserName"] == null)
            {
                filterContext.Result = new RedirectResult("/admin-giris");

            }
            base.OnActionExecuting(filterContext);
        }
    }
}