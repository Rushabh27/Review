using System;   
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using review.Models;

namespace review.Controllers
{
    public class reviewsController : Controller
    {
        private reviewmodeldb db = new reviewmodeldb();

        // GET: reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Product).Include(r => r.user);
            return View(reviews.ToList());
        }

        // GET: reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product pro = db.Products.Find(id);
            Review rev = db.Reviews.Find(id);
            var rev1 = db.Reviews.OrderByDescending(p=>p.Id).Where(d => d.productId == id).ToList();
            ViewBag.pass = rev1;
            ViewBag.c = rev1.Count();
            var tup = new Tuple<product, Review>(pro, rev);
            ViewBag.cat = pro.catId;
            ViewBag.sub = pro.subcatId;
            return View(tup);
        }
        [HttpPost]
        public ActionResult Details(string content, int rating, int Id)
        {
            //var a = content + " " + rating;
            
            if (Session["email"] == null)
            {
                return RedirectToAction("Login","Account");
            }
            else
            {
                var mail = Session["email"].ToString();
                // user user = db.Users.Find(Id);
                var ids = db.Users.FirstOrDefault(d => d.email == mail);
                db.Reviews.Add(new Review
                {
                    content = content,
                    rating = rating,
                    productId = Id,
                    userId = ids.id,
                    dape_post = DateTime.Now
                });
                db.SaveChanges();
            }
                //var ids = (from x in db.Users where email == mail select x).FirstOrDefault();
        return RedirectToAction("Details");
            
        }

        // GET: reviews/Create
        public ActionResult Create()
        {
            ViewBag.productId = new SelectList(db.Products, "Id", "productname");
            ViewBag.userId = new SelectList(db.Users, "id", "name");
            return View();
        }

        // POST: reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,content,rating,productId,userId")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productId = new SelectList(db.Products, "Id", "productname", review.productId);
            ViewBag.userId = new SelectList(db.Users, "id", "name", review.userId);
            return View(review);
        }

        // GET: reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.productId = new SelectList(db.Products, "Id", "productname", review.productId);
            ViewBag.userId = new SelectList(db.Users, "id", "name", review.userId);
            return View(review);
        }

        // POST: reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,content,rating,productId,userId")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productId = new SelectList(db.Products, "Id", "productname", review.productId);
            ViewBag.userId = new SelectList(db.Users, "id", "name", review.userId);
            return View(review);
        }

        // GET: reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
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
