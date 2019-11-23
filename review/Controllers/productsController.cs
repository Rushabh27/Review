﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using review.Models;

namespace review.Controllers
{
    public class productsController : Controller
    {
        private reviewmodeldb db = new reviewmodeldb();

        // GET: products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        
        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //product pro = db.Products.Find(id);
            var pt = db.Products.Where(d => d.subcatId == id).ToList();
            //Review Rev = db.Reviews.Find(id);
            //var tup = new Tuple<product, Review>(pt, Rev);
            
            return View(pt);
        }
        
        
        // GET: products/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase uploadfile)
        {
            if (uploadfile != null && uploadfile.FileName!="")
            {
                string pic = Path.GetFileName(uploadfile.FileName);
                string p = Path.Combine(Server.MapPath("~/Content/images/"), pic);
                uploadfile.SaveAs(p);
                ViewBag.fil = "~/Content/images/" + pic;
                
            }
            else
            {
                ViewBag.fil = "null";
            }
            return View("Create");
        }
        // POST: products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,productname,img,catId,subcatId")] product product)
        {
            if (ModelState.IsValid)
            {
                ViewBag.i = product.img;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                return View("Index");
            }
            
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,productname,img,catId,subcatId")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.Products.Find(id);
            db.Products.Remove(product);
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
