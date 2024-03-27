using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGiay.Models;
using PagedList;
using PagedList.Mvc;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace WebGiay.Controllers
{
    public class HomeController : Controller
    {
        DBSportStoreEntities1 database = new DBSportStoreEntities1();
        public ActionResult Index(string category, int? page, double min =double.MinValue, double max = double.MaxValue)
        {
            int pagesize = 4;
            int pageNum = (page ?? 1);
            if (category == null )
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro);
                return View(productList.ToPagedList(pageNum,pagesize));
            }
            else 
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == category);
                return View(productList.ToPagedList(pageNum, pagesize));
            }

        }

        public ActionResult Search(string SearchString, int? page, double min = double.MinValue, double max = double.MaxValue)
        {
            int pagesize = 4;
            int pageNum = (page ?? 1);
            if (SearchString == null)
            {
                var productList = database.Products.OrderByDescending(x => x.NamePro);
                return View(productList.ToPagedList(pageNum, pagesize));
            }
            else
            {
                var productList = database.Products.OrderByDescending(s=>s.NamePro).Where(s=>s.NamePro ==SearchString);
                return View(productList.ToPagedList(pageNum, pagesize));
            }
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