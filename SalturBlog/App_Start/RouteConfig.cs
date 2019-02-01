using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SalturBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
          name: "Search",
          url: "arama-sonuclari",
          defaults: new { controller = "Search", action = "Index"  }
          );
            routes.MapRoute(
           name: "Login",
           url: "admin-giris",
           defaults: new { controller = "Login", action = "Index",id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "Logout",
           url: "cikis-yap",
           defaults: new { controller = "Login", action = "LogOut", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "Adminler",
            url: "adminler",
            defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "AdminEkle",
           url: "admin-ekle",
          defaults: new { controller = "Adminler", action = "Create" }
          );
            routes.MapRoute(
          name: "Adminlsitesi",
          url: "admin-listesi",
         defaults: new { controller = "Adminler", action = "Index" }
         );
            routes.MapRoute(
          name: "AdminEdit",
          url: "admin-ayarlari/duzenle/{id}",
          defaults: new { controller = "Adminler", action = "Edit", id = "" }
          );
            routes.MapRoute(
           name: "MakaleListesi",
           url: "makale-listesi",
          defaults: new { controller = "Article", action = "Index" }
          );
            routes.MapRoute(
         name: "MakaleEkle",
         url: "makale-ekle",
         defaults: new { controller = "Article", action = "Create" }
         );

            routes.MapRoute(
            name: "ArticleEdit",
            url: "makale/duzenle/{id}",
            defaults: new { controller = "Article", action = "Edit", id = "" }
            );
            routes.MapRoute(
             name: "GaleriListesi",
             url: "galeri-listesi",
            defaults: new { controller = "AdminGallery", action = "Index" }
            );
            routes.MapRoute(
            name: "GaleriEkle",
            url: "galeri-resmi-ekle",
           defaults: new { controller = "AdminGallery", action = "Create" }
           );
            routes.MapRoute(
            name: "AuthorDetail",
            url: "yazarlar/{authorName}",
            defaults: new { controller = "Author", action = "Index", String = "" }
            );
            routes.MapRoute(
            name: "ArticleDetail",
            url: "{catagory}/{articleLinkUrl}",
            defaults: new { controller = "Home", action = "ArticleDetail", String = "" }
            );
        
            routes.MapRoute(
            name: "Category",
            url: "{catagoryName}",
            defaults: new { controller = "Catagory", action = "Index", String = "" }
            );
      
            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
