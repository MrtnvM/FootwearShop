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
    public class MotionsController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        private class MotionData
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public DateTime ActionDate { get; set; }

            public int ActivityId { get; set; }
            public string ActivityName { get; set; }

            public int FootwearId { get; set; }
            public int FootwearSize { get; set; }
            public double Price { get; set; }

            public int TypeId { get; set; }
            public string TypeName { get; set; }

            public int MakerId { get; set; }
            public string MakerName { get; set; }            

            public static List<Motion> GetMotionList(List<MotionData> data)
            {
                List<Motion> motions = new List<Motion>(data.Count);

                foreach (MotionData md in data)
                {
                    Motion m = new Motion();
                    m.Activity = new Activity();
                    m.Footwear = new Footwear();
                    m.Footwear.Maker = new Maker();
                    m.Footwear.FootwearType = new FootwearType();

                    m.Id = md.Id;
                    m.ActionDate = md.ActionDate;                    
                    m.Amount = md.Amount;
                    m.ActivityId = md.ActivityId;
                    m.FootwearId = md.FootwearId;

                    m.Activity.Id = md.ActivityId;
                    m.Activity.Name = md.ActivityName;

                    m.Footwear.Id = md.FootwearId;
                    m.Footwear.MakerId = md.MakerId;
                    m.Footwear.TypeId = md.TypeId;
                    m.Footwear.FootwearSize = md.FootwearSize;
                    m.Footwear.Price = md.Price;

                    m.Footwear.Maker.Id = md.MakerId;
                    m.Footwear.Maker.Name = md.MakerName;

                    m.Footwear.FootwearType.Id = md.TypeId;
                    m.Footwear.FootwearType.Name = md.TypeName;

                    motions.Add(m);
                }

                return motions;
            }
        }

        public ActionResult Index()
        {
            string sql = "SELECT " +
                            "[Motion].[Id], [Motion].[Amount], [Motion].[ActionDate], " +
                            "[Footwear].[Id] AS [FootwearId], [Footwear].[FootwearSize], [Footwear].[Price], " +
                            "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName], " +
                            "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName], " +
                            "[Activity].[Id] AS [ActivityId], [Activity].[Name] AS [ActivityName] " +
                         "FROM [Footwear], [FootwearType], [Maker], [Activity], [Motion] " +
                         "WHERE [Motion].[ActivityId] = [Activity].[Id] AND " +
                               "[Motion].[FootwearId] = [Footwear].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] ";
            List<MotionData> motionData = db.Database.SqlQuery<MotionData>(sql).ToList();
            List<Motion> motions = MotionData.GetMotionList(motionData);
            return View(motions);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT " +
                            "[Motion].[Id], [Motion].[Amount], [Motion].[ActionDate], " +
                            "[Footwear].[Id] AS [FootwearId], [Footwear].[FootwearSize], [Footwear].[Price], " +
                            "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName], " +
                            "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName], " +
                            "[Activity].[Id] AS [ActivityId], [Activity].[Name] AS [ActivityName] " +
                         "FROM [Footwear], [FootwearType], [Maker], [Activity], [Motion] " +
                         "WHERE [Motion].[ActivityId] = [Activity].[Id] AND " +
                               "[Motion].[FootwearId] = [Footwear].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Motion].[Id] = @id ";
            List<MotionData> motionData = db.Database.SqlQuery<MotionData>(sql, recordId).ToList();
            Motion motion = MotionData.GetMotionList(motionData).FirstOrDefault();
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
                var recordAmount = new SqlParameter("@amount", motion.Amount);
                var recordActionDate = new SqlParameter("@actionDate", motion.ActionDate);
                var recordFootwearId = new SqlParameter("@footwearId", motion.FootwearId);
                var recordActivityId = new SqlParameter("@activityId", motion.ActivityId);
                string sql = "INSERT INTO [Motion] (Amount, ActionDate, FootwearId, ActivityId) " +
                             "VALUES (@amount, @actionDate, @footwearId, @activityId) ";
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT " +
                            "[Motion].[Id], [Motion].[Amount], [Motion].[ActionDate], " +
                            "[Footwear].[Id] AS [FootwearId], [Footwear].[FootwearSize], [Footwear].[Price], " +
                            "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName], " +
                            "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName], " +
                            "[Activity].[Id] AS [ActivityId], [Activity].[Name] AS [ActivityName] " +
                         "FROM  [Footwear], [FootwearType], [Maker], [Activity], [Motion] " +
                         "WHERE [Motion].[ActivityId] = [Activity].[Id] AND " +
                               "[Motion].[FootwearId] = [Footwear].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Motion].[Id] = @id ";
            List<MotionData> motionData = db.Database.SqlQuery<MotionData>(sql, recordId).ToList();
            Motion motion = MotionData.GetMotionList(motionData).FirstOrDefault();
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
                var recordId = new SqlParameter("@id", motion.Id);
                var recordAmount = new SqlParameter("@amount", motion.Amount);
                var recordActionDate = new SqlParameter("@actionDate", motion.ActionDate);
                var recordFootwearId = new SqlParameter("@footwearId", motion.FootwearId);
                var recordActivityId = new SqlParameter("@activityId", motion.ActivityId);
                string sql = "UPDETE [Motion] " +
                             "SET [Amount] = @amount, " + 
                                 "[ActionDate] = @actionDate, " +
                                 "[FootwearId] = @footwearId, " +
                                 "[ActivityId] = @activityId " +
                             "WHERE [Id] = @id ";
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
            var recordId = new SqlParameter("@id", id);
            string sql = "SELECT " +
                            "[Motion].[Id], [Motion].[Amount], [Motion].[ActionDate], " +
                            "[Footwear].[Id] AS [FootwearId], [Footwear].[FootwearSize], [Footwear].[Price], " +
                            "[Maker].[Id] AS [MakerId], [Maker].[Name] AS [MakerName], " +
                            "[FootwearType].[Id] AS [TypeId], [FootwearType].[Name] AS [TypeName], " +
                            "[Activity].[Id] AS [ActivityId], [Activity].[Name] AS [ActivityName] " +
                         "FROM  [Footwear], [FootwearType], [Maker], [Activity], [Motion] " +
                         "WHERE [Motion].[ActivityId] = [Activity].[Id] AND " +
                               "[Motion].[FootwearId] = [Footwear].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Motion].[Id] = @id ";
            List<MotionData> motionData = db.Database.SqlQuery<MotionData>(sql, recordId).ToList();
            Motion motion = MotionData.GetMotionList(motionData).FirstOrDefault();
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
            var recordId = new SqlParameter("@id", id);
            string sql = "DELETE FROM [Motion] " +
                         "WHERE [Id] = @id ";
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
