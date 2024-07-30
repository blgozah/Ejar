using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using System.Web.Security;
using Ejar_new.Models;
namespace Ejar_new.Controllers
{
    public class HomeController : Controller
    {
        private readonly EjarEntities db=new EjarEntities();
        
        public ActionResult Index()
        {
            return View();
        }
      [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                bool IsValidUser = db.Users
               .Any(u => u.user_name == user
               .user_name.ToLower() && user.password == user.password);

                if (IsValidUser )
                {
                    FormsAuthentication.SetAuthCookie(user.user_name, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User registerUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(registerUser);
                db.SaveChanges();
                return RedirectToAction("Login");

            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Apartment()
        {
            var apartments = db.Apartments.Include(a => a.Area);
            return View(apartments.ToList());
            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string search)
        {
            var result = db.Apartments.Where(a => a.address.Contains(search)
            || a.Area.area_name.Contains(search) ).ToList();
            return View(result);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }
        public ActionResult Create()
        {
            ViewBag.apartment_id = new SelectList(db.Apartments, "apartment_id", "address");
            return View();
        }

        // POST: reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(reservation reservation,int id, string Password1, string Text1,int period)
        {
            if (ModelState.IsValid)
            {
                bool IsValidUser = db.Users.Any(u => u.user_name == Text1 && u.password == Password1);

                if (IsValidUser)
                {
                    var get_id = db.Users.Where(u=>u.user_name==Text1).ToList();
                    reservation.period = period;
                    reservation.user_id = get_id[0].id;
                    reservation.period_start = DateTime.Now;
                    reservation.period_end = reservation.period_start.AddMonths(period);
                    reservation.apartment_id = id;
                    db.reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }



            ViewBag.apartment_id = new SelectList(db.Apartments, "apartment_id", "address", reservation.apartment_id);
            return View(reservation);
        }

    }
}