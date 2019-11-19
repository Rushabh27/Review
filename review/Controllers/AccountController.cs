﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using review.Models;
namespace review.Controllers
{
    public class AccountController : Controller
    {
        private reviewmodeldb db = new reviewmodeldb();
        // GET: Account
        public ActionResult Index()
        {
            var m = Session["email"].ToString();
            var ids = db.Users.FirstOrDefault(d => d.email == m);
            var r = db.Reviews.FirstOrDefault(d => d.userId == ids.id);
            var re = db.Reviews.Where(d => d.userId == ids.id);
            ViewBag.view = re;
            var t = new Tuple<user, Review>(ids, r);
            return View(t);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review rev = db.Reviews.Find(id);
            if (rev == null)
            {
                return HttpNotFound();
            }
            return View(rev);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review r = db.Reviews.Find(id);
            db.Reviews.Remove(r);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(adminn model)
        {
            using (var context = new reviewmodeldb())
            {
                bool isValid = context.Admins.Any(x => x.email == model.email && x.password == model.password);
                if (isValid)
                {
                    Session["admin"] = model.email;
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    return RedirectToAction("Index", "categories");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
            //return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user model)
        {
            using (var context = new reviewmodeldb())
            {
                bool isValid = context.Users.Any(x => x.email == model.email && x.Password == model.Password);
                if (isValid)
                {
                    Session["email"] = model.email;
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    return RedirectToAction("Index", "categories");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
        }
        // GET: Register
        public ActionResult Registeruser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registeruser(user model)
        {
            try
            {
                var context = new reviewmodeldb();
                context.Users.Add(model);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            // return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}