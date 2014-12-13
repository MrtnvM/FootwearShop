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
    public class MotionsController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            var motions = db.Motions.Include(m => m.Activity).Include(m => m.Footwear);
            return View(motions.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motion motion = db.Motions.Find(id);
            if (motion == null)
            {
                return HttpNotFound();
            }
            return View(motion);
        }

        public ActionResult Create()
        {
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            ViewBag.FootwearId = new SelectList(db.Footwears, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FootwearId,Amount,ActionDate,ActivityId")] Motion motion)
        {
            if (ModelState.IsValid)
            {
                db.Motions.Add(motion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", motion.ActivityId);
            ViewBag.FootwearId = new SelectList(db.Footwears, "Id", "Id", motion.FootwearId);
            return View(motion);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motion motion = db.Motions.Find(id);
            if (motion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", motion.ActivityId);
            ViewBag.FootwearId = new SelectList(db.Footwears, "Id", "Id", motion.FootwearId);
            return View(motion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FootwearId,Amount,ActionDate,ActivityId")] Motion motion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", motion.ActivityId);
            ViewBag.FootwearId = new SelectList(db.Footwears, "Id", "Id", motion.FootwearId);
            return View(motion);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motion motion = db.Motions.Find(id);
            if (motion == null)
            {
                return HttpNotFound();
            }
            return View(motion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Motion motion = db.Motions.Find(id);
            db.Motions.Remove(motion);
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
