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
    public class FootwearTypesController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public ActionResult Index()
        {
            string sql = "SELECT * FROM [dbo].[FootwearType] ";
            IEnumerable<FootwearType> types = db.Database.SqlQuery<FootwearType>(sql);
            return View(types);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[FootwearType] " +
                         "WHERE Id = @id ";
            FootwearType footwearType = db.Database.SqlQuery<FootwearType>(sql, recordId).FirstOrDefault();
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
                var recordName = new SqlParameter("@name", footwearType.Name);
                string sql = "INSERT INTO [dbo].[Footweartype] (Name) " +
                             "VALUES (@name) ";
                db.Database.ExecuteSqlCommand(sql, recordName);
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[FootwearType] " +
                         "WHERE Id = @id ";
            FootwearType footwearType = db.Database.SqlQuery<FootwearType>(sql, recordId).FirstOrDefault();
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
                var recordId = new SqlParameter("@id", footwearType.Id);
                var recordName = new SqlParameter("@name", footwearType.Name);
                string sql = "UPDATE [dbo].[FootwearType] " +
                             "SET Name = @name " +
                             "WHERE Id = @id ";
                db.Database.ExecuteSqlCommand(sql, recordId, recordName);
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT * FROM [dbo].[FootwearType] " +
                         "WHERE Id = @id ";
            FootwearType footwearType = db.Database.SqlQuery<FootwearType>(sql, recordId).FirstOrDefault();
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
            var recordId = new SqlParameter("@id", id);
            string sql = "DELETE FROM [dbo].[FootwearType] " +
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
