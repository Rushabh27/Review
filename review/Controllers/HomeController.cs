using review.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace review.Controllers
{
    public class HomeController : Controller
    {
        private reviewmodeldb db = new reviewmodeldb();
        public ActionResult Search(string search)
        {
            if (search == "")
            {
                ViewBag.flag = "false1";
            }
            else
            {
                if (db.Products.FirstOrDefault(d => d.productname == search) == null)
                {
                    ViewBag.flag = "false2";
                }
                else
                {
             
                    return View(db.Products.FirstOrDefault(d => d.productname == search));
                }
            }
            return View(db.Products.FirstOrDefault(d => d.productname == search));
        }

        public ActionResult Index()
        {
            var pro = db.Products.OrderByDescending(r => r.Id).Take(4);
            ViewBag.p = pro;
            var lastFiveProducts = db.Reviews.OrderByDescending(p=>p.Id).Take(2);
            return View(lastFiveProducts);
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
    }
}