using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FootwearShop.Models;

namespace FootwearShop.Controllers
{
    public class FootwearTypesController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            return View(db.FootwearTypes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootwearType footwearType = db.FootwearTypes.Find(id);
            if (footwearType == null)
            {
                return HttpNotFound();
            }
            return View(footwearType);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] FootwearType footwearType)
        {
            if (ModelState.IsValid)
            {
                db.FootwearTypes.Add(footwearType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(footwearType);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootwearType footwearType = db.FootwearTypes.Find(id);
            if (footwearType == null)
            {
                return HttpNotFound();
            }
            return View(footwearType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] FootwearType footwearType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footwearType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(footwearType);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FootwearType footwearType = db.FootwearTypes.Find(id);
            if (footwearType == null)
            {
                return HttpNotFound();
            }
            return View(footwearType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FootwearType footwearType = db.FootwearTypes.Find(id);
            db.FootwearTypes.Remove(footwearType);
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
