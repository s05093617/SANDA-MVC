using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Music_Station0730.Models;
using System.Net;

namespace Music_Station0730.Controllers
{
    public class VIPController : Controller
    {
        private Will_LinEntities2 db = new Will_LinEntities2();
        // GET: Discuss2
        public ActionResult Discuss2(int? id)
        {
            ViewBag.id = id;
            var content = db.Content.ToList();
            if (id.ToString() != "" && id != null)
            {
                content = content.Where(x => x.id.ToString() == id.ToString()).ToList();
                if (content.Count.ToString() =="0")
                {
                    ViewBag.cont = "0";
                }
                else
                {
                    ViewBag.cont = content.Count.ToString();
                }
            }
            return View(content);

        }
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Playerr playerr = db.Playerr.Find(id);
        //    if (playerr == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(playerr);
        //}
        //[HttpPost, ActionName("Discuss2")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Discuss2Confirmed([Bind(Include = "ContentID,id,UserName,Content1")] Content content)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Content.Remove(content);
        //        db.SaveChanges();
        //        return RedirectToAction("Discuss2");
        //    }
        //}
        protected override void Dispose(bool disposing) //這是做什麼的?
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Discuss3()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Discuss3([Bind(Include = "ContentID,id,UserName,Content1")] Content content)
        {
            if (ModelState.IsValid)
            {
                db.Content.Add(content);
                db.SaveChanges();
                return RedirectToAction("Discuss3");
            }

            return View(content);
        }
    }
}