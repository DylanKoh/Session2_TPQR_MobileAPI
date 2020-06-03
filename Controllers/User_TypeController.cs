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
    public class User_TypeController : Controller
    {
        private Session2Entities db = new Session2Entities();

        public User_TypeController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // POST: User_Type
        [HttpPost]
        public ActionResult Index()
        {
            return Json(db.User_Type.ToList());
        }

        // POST: User_Type/Details/5
        [HttpPost]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Type user_Type = db.User_Type.Find(id);
            if (user_Type == null)
            {
                return HttpNotFound();
            }
            return Json(user_Type);
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
