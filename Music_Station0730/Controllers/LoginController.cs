using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Music_Station0730.Models;

namespace Music_Station0730.Controllers
{
    public class LoginController : Controller
    {
        private Will_LinEntities2 db = new Will_LinEntities2();
        // GET: Login

        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserName,PassWord,Email,level")] User User)
        {
            User.level = "2";
            if (ModelState.IsValid)
            {
                db.User.Add(User);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Login_UserName, string Login_PassWord)
        {
            var UN = Login_UserName;
            var PW = Login_PassWord;
            if (UN != "" && UN != null && PW != "" && PW != null)
            {
                var check = db.User.Where(x => x.UserName == UN && x.PassWord == PW).FirstOrDefault();
                if (check == null)
                {
                    ViewBag.error = "帳號或密碼錯誤";
                    return View();
                }
                else
                {
                    Session["UserName"] = UN;
                    Session["level"] = check.level;
                    return RedirectToAction("Index", "Home");
                }
                //UN_check = UN_check.Where(x => x.UserName.ToString() == UN).ToList();
                //UPW_check = UPW_check.Where(c => c.PassWord.ToString() == PW).ToList();
            }
            ViewBag.error = "請輸入帳號或密碼";
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            Session["level"] = null;
            return View();
        }
    }
}