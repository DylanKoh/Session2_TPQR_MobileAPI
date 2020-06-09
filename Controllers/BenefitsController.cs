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
    public class BenefitsController : Controller
    {
        private Session2Entities db = new Session2Entities();


        // POST: Benefits/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "benefitId,packageIdFK,benefitName")] Benefit benefit)
        {
            if (ModelState.IsValid)
            {
                db.Benefits.Add(benefit);
                db.SaveChanges();
                return Json("Benefits added!");
            }
            return Json("Unable to add Benefits!");
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
