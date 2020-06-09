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
    public class BookingsController : Controller
    {
        private Session2Entities db = new Session2Entities();

        public BookingsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // POST: Bookings
        [HttpPost]
        public ActionResult Index()
        {
            var bookings = db.Bookings;
            return Json(bookings.ToList());
        }

        // POST: Bookings/GetUserBooking?userID={}
        [HttpPost]
        public ActionResult GetUserBooking(string userID)
        {
            var getUserBooking = (from x in db.Bookings
                                  where x.userIdFK == userID && x.status != "Pending"
                                  select x);
            return Json(getUserBooking.ToList());
        }


        // POST: Bookings/Create.
        [HttpPost]
        public ActionResult Create([Bind(Include = "bookingId,userIdFK,packageIdFK,quantityBooked,status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return Json("Booking successful!");
            }

            return Json("Unable to book package! Please try again later!");
        }


        // POST: Bookings/Edit/5?quantity={}
        [HttpPost]
        public ActionResult Edit(int id, int quantity)
        {
            try
            {
                var getBooking = db.Bookings.Where(x => x.bookingId == id).Select(x => x).FirstOrDefault();
                var getPackage = db.Packages.Where(x => x.packageId == getBooking.packageIdFK).Select(x => x).FirstOrDefault();
                if (getPackage.packageQuantity + getBooking.quantityBooked - quantity < 0)
                {
                    return Json("Package do not have enough quantity for new amount!");
                }
                else
                {
                    getPackage.packageQuantity += getBooking.quantityBooked;
                    db.SaveChanges();
                    getBooking.quantityBooked = quantity;
                    getBooking.status = "Pending";
                    db.SaveChanges();
                    return Json("Booking edited successfully!");
                }
                
            }
            catch (Exception)
            {

                return Json("Unable to edit booking!");
            }
            
        }

        // POST: Bookings/ApproveBookings/5?value={}
        [HttpPost]
        public ActionResult ApproveBookings(int id, string value)
        {
            if (value == "Approved")
            {
                var getBooking = db.Bookings.Find(id);
                var getPackage = db.Packages.Where(x => x.packageId == getBooking.packageIdFK).Select(x => x).FirstOrDefault();
                if (getPackage.packageQuantity - getBooking.quantityBooked < 0)
                {
                    return Json("Unable to approve booking! Package quantity is not enough!");
                }
                else
                {
                    getPackage.packageQuantity -= getBooking.quantityBooked;
                    getBooking.status = "Approved";
                    db.SaveChanges();
                    return Json("Booking approved successfully!");
                }
                
            }
            else
            {
                var getBooking = db.Bookings.Find(id);
                getBooking.status = "Rejected";
                db.SaveChanges();
                return Json("Booking rejected successfully!");
            }
        }

        // POST: Bookings/GetManagerView
        [HttpPost]
        public ActionResult GetManagerView()
        {
            var getBookings = (from x in db.Bookings
                               join y in db.Packages on x.packageIdFK equals y.packageId
                               join z in db.Users on x.userIdFK equals z.userId
                               select new { BookingID = x.bookingId, CompanyName = z.name, PackageName = y.packageName, Status = x.status })
                               .OrderBy(x => x.Status == "Rejected")
                               .ThenBy(x => x.Status == "Approved")
                               .ThenBy(x => x.Status == "Pending");
            return Json(getBookings.ToList());
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            var getPackage = db.Packages.Where(x => x.packageId == booking.packageIdFK).Select(x => x).FirstOrDefault();
            getPackage.packageQuantity += booking.quantityBooked;
            db.SaveChanges();
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return Json("Booking removed successfully!");
        }

        // POST: Bookings/GetSpecificBookings?userID={}
        [HttpPost]
        public ActionResult GetSpecificBookings(string userID)
        {
            var getApproved = (from x in db.Bookings
                               where x.userIdFK == userID && x.status == "Approved"
                               join y in db.Packages on x.packageIdFK equals y.packageId
                               select new
                               {
                                   BookingID = x.bookingId,
                                   PackageTier = y.packageTier,
                                   PackageName = y.packageName,
                                   PackageValue = y.packageValue,
                                   QuantityBooked = x.quantityBooked,
                                   SubTotal = x.quantityBooked * y.packageValue
                               });
            return Json(getApproved.ToList());
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
