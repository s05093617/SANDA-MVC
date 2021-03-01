using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Music_Station0730.Models;
using System.Data;
using Music_Station0730.Helpers;

namespace Music_Station0730.Controllers
{
    public class HomeController : Controller
    {
        private Will_LinEntities2 db = new Will_LinEntities2();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Baroque()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Classical()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Romantic()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Form(string aa, string bb, string cc, string mode = "")
        {
            var Playerr = db.Playerr.ToList();
            ViewBag.select = Playerr.Select(x => x.style.ToString()).Distinct().ToList();
            if (aa != "" && aa != null)
            {
                Playerr = Playerr.Where(x => x.style.ToString() == aa).ToList();
            }
            if (bb != "" && bb != null)
            {
                Playerr = Playerr.Where(x => x.PlayName.ToString().Contains(bb)).ToList();
            }
            if (cc == null || cc == "new" || cc == "")
            {
                Playerr = Playerr.OrderByDescending(x => x.PlayerID).ToList();
                ViewBag.cc_change = "new";
            }
            else
            {
                Playerr = Playerr.OrderBy(x => x.PlayerID).ToList();
                ViewBag.cc_change = "old";
            }
            //if (mode == "export")
            //{
            //    Playerr = Playerr.Where(x => x.style.ToString() == Session["aa"].ToString()).ToList();
            //    Playerr = Playerr.Where(x => x.PlayName.ToString().Contains(Session["bb"].ToString())).ToList();
            //    if (Session["cc"].ToString() == null || Session["cc"].ToString() == "new" || Session["cc"].ToString() == "")
            //    {
            //        Playerr = Playerr.OrderByDescending(x => x.PlayerID).ToList();
            //        ViewBag.cc_change = "new";
            //    }
            //    else
            //    {
            //        Playerr = Playerr.OrderBy(x => x.PlayerID).ToList();
            //        ViewBag.cc_change = "old";
            //    }
            //    /*此為報表的程式碼*/
            //    DataTable dt1 = new DataTable();
            //    DataRow row;
            //    dt1.Columns.Add("style");
            //    dt1.Columns.Add("PlayName");
            //    dt1.Columns.Add("SongName");
            //    foreach (var item in Playerr)
            //    {
            //        row = dt1.NewRow();
            //        row["style"] = item.style.ToString();
            //        row["PlayName"] = item.PlayName.ToString();
            //        row["SongName"] = item.SongName.ToString();
            //        dt1.Rows.Add(row);
            //    }
            //    DateTime now = DateTime.Now;
            //    string today = now.Year.ToString() + "_" + now.Month.ToString("00") + "_" + now.Day.ToString("00");
            //    new ExcelFactory().ExportExcel("音樂總覽" + today, dt1, "/excel/Lin.xlsx", Response);

            //}
            //if (mode == "show")
            //{
            //    ViewBag.mode = "show";
            //}

            return View(Playerr);
        }
        // GET: Playerrs/discuss/5
        //youtube影片+留言板 顯示留言
        public ActionResult Discuss(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playerr playerr = db.Playerr.Find(id);
            if (playerr == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            var content = db.Content.Where(x => x.id.ToString() == id.ToString()).ToList();
            if (content.Count.ToString() == "0")
            {
                ViewBag.cont = null;
            }
            else
            {
                ViewBag.cont = content;
            }
            return View(playerr);
        }
        //留言板 留言新刪修
        [HttpPost]
        public ActionResult Discuss(string btn_move, int DContentID, string DContent1, [Bind(Include = "ContentID,id,UserName,Content1")] Content content)
        {
            if (btn_move == "Create")
            {
                db.Content.Add(content);
                db.SaveChanges();
                return RedirectToAction("Discuss");
            }
            else
            {
                switch (btn_move)
                {
                    case "Delete":
                        content = db.Content.Find(DContentID);
                        db.Content.Remove(content);
                        db.SaveChanges();
                        return RedirectToAction("Discuss");
                        break;
                    case "Edit":
                        content = db.Content.Find(DContentID);
                        content.Content1 = DContent1;
                        db.Entry(content).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Discuss");
                        break;
                }
            }

            return View();
        }
        // GET: Playerrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playerr playerr = db.Playerr.Find(id);
            if (playerr == null)
            {
                return HttpNotFound();
            }

            return View(playerr);
        }

        // GET: Playerrs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playerrs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlayerID,style,PlayName,SongName,YT_video")] Playerr playerr)
        {
            if (ModelState.IsValid)
            {
                db.Playerr.Add(playerr);
                db.SaveChanges();
                return RedirectToAction("Form");
            }

            return View(playerr);
        }

        // GET: Playerrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playerr playerr = db.Playerr.Find(id);
            if (playerr == null)
            {
                return HttpNotFound();
            }
            return View(playerr);
        }

        // POST: Playerrs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayerID,style,PlayName,SongName,YT_video")] Playerr playerr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Form");
            }
            return View(playerr);
        }

        // GET: Playerrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playerr playerr = db.Playerr.Find(id);
            if (playerr == null)
            {
                return HttpNotFound();
            }
            return View(playerr);
        }

        // POST: Playerrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Playerr playerr = db.Playerr.Find(id);
            db.Playerr.Remove(playerr);
            db.SaveChanges();
            return RedirectToAction("Form");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult DoIt()
        {
            //將上傳者/旋律/間隔回傳資料庫
            return View();
        }
    }
}