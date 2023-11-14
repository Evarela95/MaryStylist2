using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ImageController : Controller
    {
        private string imagePath = "~/images/";

        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var imageDirectory = Server.MapPath(imagePath);
            var imageNames = Directory.GetFiles(imageDirectory).Select(Path.GetFileName).ToList();
            return View(imageNames);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(imagePath), fileName);

                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string imageName)
        {
            var path = Path.Combine(Server.MapPath(imagePath), imageName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Galeria()
        {
            var imageDirectory = Server.MapPath("~/images");
            var imageList = Directory.GetFiles(imageDirectory)
                .Select(Path.GetFileName)
                .ToList();

            return View(imageList);
        }


      
    }
}