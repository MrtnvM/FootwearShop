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
    public class MakersController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            string sql = "SELECT * FROM [dbo].[Maker]";
            IEnumerable<Maker> makers = db.Database.SqlQuery<Maker>(sql);
            return View(makers);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[Maker] " +
                         "WHERE Id = @id";
            Maker maker = db.Database.SqlQuery<Maker>(sql, recordId).FirstOrDefault();
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Maker maker)
        {
            if (ModelState.IsValid)
            {
                var recordName = new SqlParameter("@name", maker.Name);
                string sql = "INSERT INTO [dbo].[Maker] (Name) " +
                             "VALUES (@name)";
                db.Database.ExecuteSqlCommand(sql, recordName);
                return RedirectToAction("Index");
            }

            return View(maker);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[Maker] " +
                         "WHERE Id = @id";
            Maker maker = db.Database.SqlQuery<Maker>(sql, recordId).FirstOrDefault();
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Maker maker)
        {
            if (ModelState.IsValid)
            {
                var recordId = new SqlParameter("@id", maker.Id);
                var recordName = new SqlParameter("@name", maker.Name);
                string sql = "UPDATE [dbo].[Maker] " +
                             "SET Name = @name " +
                             "WHERE Id = @id ";
                db.Database.ExecuteSqlCommand(sql, recordId, recordName);
                return RedirectToAction("Index");
            }
            return View(maker);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[Maker] " +
                         "WHERE Id = @id";
            Maker maker = db.Database.SqlQuery<Maker>(sql, recordId).FirstOrDefault();
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var recordId = new SqlParameter("@id", id);
            string sql = "DELETE FROM [dbo].[Maker] " +
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
