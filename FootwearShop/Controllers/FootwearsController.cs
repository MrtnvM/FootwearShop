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
    public class FootwearsController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        private class FootwearData
        {
            public int Id { get; set; }
            public int MakerId { get; set; }
            public string MakerName { get; set; }
            public int TypeId { get; set; }
            public string TypeName { get; set; }
            public int FootwearSize { get; set; }
            public double Price { get; set; }

            public static List<Footwear> GetFootwearList(List<FootwearData> fdList)
            {
                List<Footwear> footwearList = new List<Footwear>(fdList.Count());
                foreach(FootwearData fd in fdList)
                {
                    Footwear f = new Footwear();
                    f.Maker = new Maker();
                    f.FootwearType = new FootwearType();

                    f.Id = fd.Id;
                    f.MakerId = fd.MakerId;
                    f.TypeId = fd.TypeId;
                    f.FootwearSize = fd.FootwearSize;
                    f.Price = fd.Price;

                    f.Maker.Id = fd.MakerId;
                    f.Maker.Name = fd.MakerName;

                    f.FootwearType.Id = fd.TypeId;
                    f.FootwearType.Name = fd.TypeName;

                    footwearList.Add(f);
                }
                return footwearList;
            }
        }

        public ActionResult Index()
        {
            string sql = "SELECT " +
                         "[Extant1].[Id] AS [Id], " +
                         "[Extant1].[FootwearSize] AS [FootwearSize], " +
                         "[Extant1].[Price] AS [Price], " +
                         "[Extant2].[Id] AS [MakerId], " +
                         "[Extant2].[Name] AS [MakerName], " + 
                         "[Extant3].[Id] AS [TypeId], " +                          
                         "[Extant3].[Name] AS [TypeName] " +
                         "FROM [dbo].[Footwear] AS [Extant1] " +
                         "INNER JOIN [dbo].[Maker] AS [Extant2] ON [Extant1].[MakerId] = [Extant2].[Id] " +
                         "INNER JOIN [dbo].[FootwearType] AS [Extant3] ON [Extant1].[TypeId] = [Extant3].[Id] ";            
            List<FootwearData> footwear = db.Database.SqlQuery<FootwearData>(sql).ToList();
            var footwears = FootwearData.GetFootwearList(footwear);
            return View(footwears);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT [Footwear].[Id], [Footwear].[FootwearSize], [Footwear].[Price], " +
                                "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName]," +
                                "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName] " +
                         "FROM  [Footwear], [Maker], [FootwearType]" +
                         "WHERE [Footwear].[Id] = @id AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] ";
            IEnumerable<FootwearData> footwearData = db.Database.SqlQuery<FootwearData>(sql, recordId);
            Footwear footwear = FootwearData.GetFootwearList(footwearData.ToList()).FirstOrDefault();
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
                var recordMakerId = new SqlParameter("@makerId", footwear.MakerId);
                var recordTypeId = new SqlParameter("@typeId", footwear.TypeId);
                var recordFootwearSize = new SqlParameter("@footwearSize", footwear.FootwearSize);
                var recordPrice = new SqlParameter("@price", footwear.Price);
                string sql = "INSERT INTO [dbo].[Footwear] (MakerId, TypeId, FootwearSize, Price) " +
                             "VALUES (@makerId, @typeId, @footwearSize, @price) ";
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT [Footwear].[Id], [Footwear].[FootwearSize], [Footwear].[Price], " +
                                "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName]," +
                                "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName] " +
                         "FROM  [Footwear], [Maker], [FootwearType]" +
                         "WHERE [Footwear].[Id] = @id AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] ";
            IEnumerable<FootwearData> footwearData = db.Database.SqlQuery<FootwearData>(sql, recordId);
            Footwear footwear = FootwearData.GetFootwearList(footwearData.ToList()).FirstOrDefault();
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
                var recordId = new SqlParameter("@id", footwear.Id);
                var recordMakerId = new SqlParameter("@makerId", footwear.MakerId);
                var recordTypeId = new SqlParameter("@typeId", footwear.TypeId);
                var recordFootwearSize = new SqlParameter("@Size", footwear.FootwearSize);
                var recordPrice = new SqlParameter("@price", footwear.Price);
                string sql = "UPDATE [dbo].[Footwear] " +
                             "SET [MakerId] = @makerId, " +
                                 "[TypeId] = @typeId, " +
                                 "[FootwearSize] = @size, " +
                                 "[Price] = @price " +
                             "WHERE [Id] = @id ";
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT [Footwear].[Id], [Footwear].[FootwearSize], [Footwear].[Price], " +
                                "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName]," +
                                "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName] " +
                         "FROM  [Footwear], [Maker], [FootwearType]" +
                         "WHERE [Footwear].[Id] = @id AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] ";
            IEnumerable<FootwearData> footwearData = db.Database.SqlQuery<FootwearData>(sql, recordId);
            Footwear footwear = FootwearData.GetFootwearList(footwearData.ToList()).FirstOrDefault();
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
            var recordId = new SqlParameter("@id", id);
            string sql = "DELETE FROM [dbo].[Footwear] " +
                         "WHERE [Id] = @id ";
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
