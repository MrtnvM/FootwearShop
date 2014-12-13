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
    public class FootwearsController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            var footwears = db.Footwears.Include(f => f.Maker).Include(f => f.FootwearType);
            return View(footwears.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footwear footwear = db.Footwears.Find(id);
            if (footwear == null)
            {
                return HttpNotFound();
            }
            return View(footwear);
        }

        public ActionResult Create()
        {
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.FootwearTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MakerId,TypeId,FootwearSize,Price")] Footwear footwear)
        {
            if (ModelState.IsValid)
            {
                db.Footwears.Add(footwear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", footwear.MakerId);
            ViewBag.TypeId = new SelectList(db.FootwearTypes, "Id", "Name", footwear.TypeId);
            return View(footwear);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footwear footwear = db.Footwears.Find(id);
            if (footwear == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", footwear.MakerId);
            ViewBag.TypeId = new SelectList(db.FootwearTypes, "Id", "Name", footwear.TypeId);
            return View(footwear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MakerId,TypeId,FootwearSize,Price")] Footwear footwear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footwear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MakerId = new SelectList(db.Makers, "Id", "Name", footwear.MakerId);
            ViewBag.TypeId = new SelectList(db.FootwearTypes, "Id", "Name", footwear.TypeId);
            return View(footwear);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footwear footwear = db.Footwears.Find(id);
            if (footwear == null)
            {
                return HttpNotFound();
            }
            return View(footwear);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Footwear footwear = db.Footwears.Find(id);
            db.Footwears.Remove(footwear);
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
