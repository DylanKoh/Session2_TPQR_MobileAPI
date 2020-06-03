using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Session2_TPQR_MobileAPI;

namespace Session2_TPQR_MobileAPI.Controllers
{
    public class UsersController : Controller
    {
        private Session2Entities db = new Session2Entities();
        public UsersController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: /Users
        public ActionResult Index()
        {
            var users = db.Users;
            return Json(users.ToList());
        }

        


        // POST: Users/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "userId,name,passwd,userTypeIdFK")] User user)
        {
            if (ModelState.IsValid)
            {
                var checkName = db.Users.Where(x => x.name == user.name).Select(x => x).FirstOrDefault();
                var checkUserID = db.Users.Where(x => x.name == user.userId).Select(x => x).FirstOrDefault();
                if (checkName != null)
                {
                    return Json("Company already has a sponsor account!");
                }
                else if (checkUserID != null)
                {
                    return Json("User ID has been used!");
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json("User created succesfully!");
                }
                
            }

            return Json("Unable to create account! If problem persist, please contact administrator!");
        }


        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "userId,name,passwd,userTypeIdFK")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json("User edited successfully!");
            }
            return Json("Unable to edit user's account! If problem persist, please contact administrator!");
        }

        // POST: Users/Login?userID=[]&password=[]
        [HttpPost]
        public ActionResult Login(string userID, string password)
        {
            var findUser = db.Users.Where(x => x.userId == userID).Select(x => x).FirstOrDefault();
            if (findUser == null)
            {
                return Json("User not found!");
            }
            else
            {
                if (findUser.passwd != password)
                {
                    return Json("Password is incorrect!");
                }
                else
                {
                    return Json(findUser);
                }
            }
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
