using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FootwearShop.Models;
using System.Data.SqlClient;

namespace FootwearShop.Controllers
{
    public class ActivitiesController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            string sql = "SELECT * FROM [dbo].[Activity] ";
            IEnumerable<Activity> activities = db.Database.SqlQuery<Activity>(sql);
            return View(activities);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sql = "SELECT * FROM [dbo].[Activity] " +
                         "WHERE Id = @id ";
            var recordId = new SqlParameter("@id", id);
            Activity activity = db.Database.SqlQuery<Activity>(sql, recordId).FirstOrDefault();
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var recordName = new SqlParameter("@name", activity.Name);
                string sql = "INSERT INTO [dbo].[Activity] (Name) " +
                             "VALUES (@name) ";
                db.Database.ExecuteSqlCommand(sql, recordName);
                return RedirectToAction("Index");
            }

            return View(activity);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[Activity] " +
                         "WHERE Id = @id ";
            Activity activity = db.Database.SqlQuery<Activity>(sql, recordId).FirstOrDefault();
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var recordId = new SqlParameter("@id", activity.Id);
                var recordName = new SqlParameter("@name", activity.Name);
                string sql = "UPDATE [dbo].[Activity] " +
                             "SET Name = @name " +
                             "WHERE Id = @id ";
                db.Database.ExecuteSqlCommand(sql, recordId, recordName);
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sql = "SELECT * FROM [dbo].[Activity] " +
                         "WHERE Id = @id ";
            var recordId = new SqlParameter("@id", id);
            Activity activity = db.Database.SqlQuery<Activity>(sql, recordId).FirstOrDefault();
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var recordId = new SqlParameter("@id", id);
            string sql = "DELETE FROM [dbo].[Activity] " +
                         "WHERE Id = @id ";
            db.Database.ExecuteSqlCommand(sql, recordId);
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
