using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootwearShop.Models;

namespace FootwearShop.Controllers
{
    public class QueriesController : Controller
    {
        private FootwearShopEntities db = new FootwearShopEntities();

        public class MakerCount
        {
            public string MakerName { get; set; }
            public int Count { get; set; }
        }

        public class IntegratedQueryClass
        {
            public int FootwearId { get; set; }
            public string MakerName { get; set; }
            public string TypeName { get; set; }
            public int FootwearSize { get; set; }
        }

        public class Joing3Table
        {
            public string MakerName { get; set; }
            public string TypeName { get; set; }
            public int FootwearSize { get; set; }
            public double Price { get; set; }
            public int Amount { get; set; }
            public DateTime ActionDate { get; set; }
            public string ActivityName { get; set; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GroupingRows()
        {
            string sql = "SELECT [Maker].[Name] AS [MakerName], COUNT(*) AS [Count] " +
                         "FROM [Maker], [Footwear] " + 
                         "WHERE [Footwear].[MakerId] = [Maker].[Id] " +
                         "GROUP BY [Maker].[Name] " +
                         "ORDER BY [Count] ";
            IEnumerable<MakerCount> makerCounts = db.Database.SqlQuery<MakerCount>(sql);
            return View(makerCounts);
        }

        public ActionResult IntegratedQuery()
        {
            string sql = "SELECT [Footwear].[Id] AS [FootwearId], [Maker].[Name] AS [MakerName], [FootwearType].[Name] AS [TypeName], [Footwear].[FootwearSize] " +
                         "FROM [Footwear], [Maker], [FootwearType], [Motion] " +
                         "WHERE [Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Footwear].[MakerId] = [Maker].[Id] AND " + 
                               "[Footwear].[Id] IN " + 
                                    "(SELECT [Motion].[Id] FROM [Motion] " +
                                    "WHERE [Motion].[Id] < 5) ";
            IEnumerable<IntegratedQueryClass> entities = db.Database.SqlQuery<IntegratedQueryClass>(sql);
            return View(entities);
        }

        public ActionResult JoingTables()
        {
            string sql = "SELECT " +
                            "[Maker].[Name] AS [MakerName], " +
                            "[FootwearType].[Name] AS [TypeName], " +
                            "[Footwear].[FootwearSize], " +
                            "[Footwear].[Price], " +
                            "[Motion].[Amount], " +
                            "[Motion].[ActionDate], " +
                            "[Activity].[Name] AS [ActivityName] " +
                         "FROM [Footwear], [Maker], [FootwearType], [Activity], [Motion] " +
                         "WHERE [Footwear].[MakerId] = [Maker].[Id] AND " +
                               "[Footwear].[TypeId] = [FootwearType].[Id] AND " +
                               "[Motion].[FootwearId] = [Footwear].[Id] AND " +
                               "[Motion].[ActivityId] = [Activity].[Id] " +
                         "ORDER BY [Motion].[ActionDate] DESC ";
            IEnumerable<Joing3Table> entities = db.Database.SqlQuery<Joing3Table>(sql);
            ViewBag.Entities = entities;
            return View();
        }
    }
}