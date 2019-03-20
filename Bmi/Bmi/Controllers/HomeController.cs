using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using Bmi.Models;

namespace Bmi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Bmi()
        {
            var claim = ClaimsPrincipal.Current.Claims;

            if (claim != null)
            {
                var currentUserHeight = claim.FirstOrDefault(x => x.Type.Equals("extension_Height"));
                var currentUserWeight = claim.FirstOrDefault(x => x.Type == "extension_Weight");
                var currentUserBmi = new CurrentUserBmiViewModel(Convert.ToDouble(currentUserHeight.Value), Convert.ToDouble(currentUserWeight.Value));
                return View(currentUserBmi);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        var stream = upload.InputStream;
                        var csvTable = new DataTable();
                        using (var csvReader = new CsvReader(new StreamReader(stream), true))
                        {
                            csvTable.Load(csvReader);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }
    }
}