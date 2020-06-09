using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

        // POST: Packages/GetCustomView
        [HttpPost]
        public ActionResult GetCustomView()
        {
            var getPackages = (from x in db.Packages
                               where x.packageQuantity > 0
                               select x).ToList();
            var getPackagesList = (from x in getPackages
                                   select new
                                   {
                                       PackageID = x.packageId,
                                       PackageName = x.packageName,
                                       AvailableQuantity = x.packageQuantity,
                                       PackageTier = x.packageTier,
                                       PackageValue = x.packageValue,
                                       Benefits = string.Join(", ", db.Benefits.Where(y => y.packageIdFK == x.packageId).Select(y => y.benefitName).ToArray())
                                   });
            return Json(getPackagesList.ToList());
        }

        // POST: Packages/GetManagerView
        [HttpPost]
        public ActionResult GetManagerView()
        {
            var getPackages = (from x in db.Packages
                               select x).ToList();
            var getPackagesList = (from x in getPackages
                                   select new
                                   {
                                       PackageID = x.packageId,
                                       PackageName = x.packageName,
                                       AvailableQuantity = x.packageQuantity,
                                       PackageTier = x.packageTier,
                                       PackageValue = x.packageValue,
                                       Benefits = string.Join(", ", db.Benefits.Where(y => y.packageIdFK == x.packageId).Select(y => y.benefitName).ToArray())
                                   });
            return Json(getPackagesList.ToList());
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
                    var newID = db.Packages.Where(x => x.packageName == package.packageName).Select(x => x.packageId).FirstOrDefault();
                    return Json(newID);
                }

            }

            return Json("An error occurred while attempting to create package! Please try again later");
        }

        // POST: Packages/GetTiers
        [HttpPost]
        public ActionResult GetTiers()
        {
            var tier = db.Packages.Select(x => x.packageTier).Distinct().OrderBy(x => x == "Bronze").ThenBy(x => x == "Silver").ThenBy(x => x == "Gold");
            return Json(tier.ToList());
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
