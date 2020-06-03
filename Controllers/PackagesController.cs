using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Session2_TPQR_MobileAPI;

namespace Session2_TPQR_MobileAPI.Controllers
{
    public class PackagesController : Controller
    {
        private Session2Entities db = new Session2Entities();

        public PackagesController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // POST: Packages
        [HttpPost]
        public ActionResult Index()
        {
            return Json(db.Packages.ToList());
        }

        // POST: Packages/Details/5
        [HttpPost]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return Json(package);
        }


        // POST: Packages/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "packageId,packageTier,packageName,packageValue,packageQuantity")] Package package)
        {
            if (ModelState.IsValid)
            {
                var checkPackage = db.Packages.Where(x => x.packageName == package.packageName).Select(x => x).FirstOrDefault();
                if (checkPackage != null)
                {
                    return Json("Package Name has been taken!");
                }
                else if (package.packageTier == "Gold" && package.packageValue < 50000)
                {
                    return Json("Package value for Gold Tier is invalid!");
                }
                else if (package.packageTier == "Silver" && (package.packageValue <= 10000 || package.packageValue >= 50000))
                {
                    return Json("Package value for Silver Tier is invalid!");
                }
                else if (package.packageTier == "Bronze" && (package.packageValue <= 0 || package.packageValue > 10000))
                {
                    return Json("Package value for Bronze Tier is invalid!");
                }
                else
                {
                    db.Packages.Add(package);
                    db.SaveChanges();
                    return Json("Package created successfully!");
                }
                
            }

            return Json("An error occurred while attempting to create package! Please try again later");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
