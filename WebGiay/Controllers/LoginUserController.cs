using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebGiay.Models;

namespace WebGiay.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities1 database = new DBSportStoreEntities1();
        // GET: LoginUser
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
            var check = database.AdminUsers.Where( s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Thông tin Login sai";
                return View("Index");
            }
            else
            {   var user = database.AdminUsers.Where(s=>s.NameUser==_user.NameUser).ToList();
                if(user.Count>0)
                { var role = user.First().RoleUser; 
                    if(role =="1")
                    {
                        database.Configuration.ValidateOnSaveEnabled = false;
                        Session["ID"] = _user.ID;
                        Session["PasswordUser"] = _user.PasswordUser;
                        Session["UsName"] = _user.NameUser;
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    return View("Index");
                }
                return View("Index");

            }
        }

        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                var check_Uname = database.AdminUsers.Where(s => s.NameUser == _user.NameUser).FirstOrDefault();
                if (check_Uname == null) 
                { 
                    database.Configuration.ValidateOnSaveEnabled=false;
                    database.AdminUsers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "Thông tin đã tồn tại";
                    return View();
                }
            }
            return View();
        }

        public ActionResult LogoutUser ()
        {
            Session.Abandon();
            return RedirectToAction("Index", "HomeAdmin");
        }
    }
}