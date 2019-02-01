using SalturBlog.Models;
using SalturBlog.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SalturBlog.Controllers
{
    public class AdminGalleryController : BaseController
    {
      
        public ActionResult Index()
        {
            return View(db.Gallery.ToList());
        }

        public ActionResult Create(Gallery gallery, IEnumerable<HttpPostedFileBase> ImageFiles)
        {
            if (ImageFiles != null)
            {
                foreach (var item in ImageFiles)
                {
                        FileInfo fileinfo = new FileInfo(item.FileName);                  
                        WebImage img = new WebImage(item.InputStream);
                        string uzanti = Guid.NewGuid().ToString() + fileinfo.Extension;
                        img.Resize(82, 82, false, true);
                        string TamYolYeri = "~/Content/img/sidebar/ins/" + uzanti;
                        img.Save(Server.MapPath(TamYolYeri));
                        gallery.ImageUrl = "/Content/img/sidebar/ins/" + uzanti;
                        db.Gallery.Add(gallery);
                        db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
          
         return View();
       }

        public ActionResult Delete(int id)
        {
            Gallery gallery = db.Gallery.Find(id);
            db.Gallery.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}