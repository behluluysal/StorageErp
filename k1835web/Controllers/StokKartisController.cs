using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using k1835web.Models;
using k1835web.Security;

namespace k1835web.Controllers
{
    public class StokKartisController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: StokKartis
        public ActionResult Index()
        {
            return View(db.stokKartis.ToList());
        }

        // GET: StokKartis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokKarti stokKarti = db.stokKartis.Find(id);
            if (stokKarti == null)
            {
                return HttpNotFound();
            }
            return View(stokKarti);
        }

        // GET: StokKartis/Create
        public ActionResult Create()
        {
            ViewBag.Depos = db.depos.ToList();
            return View();
        }

        // POST: StokKartis/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Kodu,Adi,Kdv,Fiyat,Aciklama,KayitTarihi")] StokKarti stokKarti,string depo)
        {
            string ad = SessionPersister.Username;
            var usr = db.Admins.Where(x => x.Kullaniciadi == ad).FirstOrDefault();
            ad = usr.Isim + usr.Soyisim;
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        // burada dilerseniz dosya tipine gore filtreleme yaparak sadece istediginiz dosya formatindaki dosyalari yukleyebilirsiniz
                        if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                        {
                            var fi = new FileInfo(file.FileName);
                            fileName = Path.GetFileName(file.FileName);
                            fileName = Guid.NewGuid().ToString() + fi.Extension;
                            var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                            file.SaveAs(path);
                        }
                    }
                }
                int parsed = int.Parse(depo);
                Depo dep = db.depos.Where(x => x.Id == parsed).FirstOrDefault();
                stokKarti.Depo = dep;
                stokKarti.Resim = fileName;
                stokKarti.KayitYapanAdSoyad = ad;
                db.stokKartis.Add(stokKarti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Depos = db.depos.ToList();
            return View(stokKarti);
        }

        // GET: StokKartis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokKarti stokKarti = db.stokKartis.Find(id);
            if (stokKarti == null)
            {
                return HttpNotFound();
            }
            ViewBag.Depos = db.depos.ToList();
            return View(stokKarti);
        }

        // POST: StokKartis/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Kodu,Adi,Kdv,Fiyat,Aciklama,KayitYapanAdSoyad,KayitTarihi")] StokKarti stokKarti)
        {
            if (ModelState.IsValid)
            {
                Kullanici kul = db.Admins.AsNoTracking().Where(x=>x.Kullaniciadi == SessionPersister.Username).FirstOrDefault();


                string fileName = "";
                StokKarti eski;
                eski = db.stokKartis.AsNoTracking().Where(x => x.Id == stokKarti.Id).FirstOrDefault();
                fileName = eski.Resim;
                if (Request.Files.Count > 0)
                {

                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        // burada dilerseniz dosya tipine gore filtreleme yaparak sadece istediginiz dosya formatindaki dosyalari yukleyebilirsiniz
                        if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                        {
                            var fi = new FileInfo(file.FileName);
                            fileName = Path.GetFileName(file.FileName);
                            fileName = Guid.NewGuid().ToString() + fi.Extension;
                            var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                            file.SaveAs(path);

                            string eskiFile = eski.Resim;
                            path = Path.Combine(Server.MapPath("~/img/"), eskiFile);
                            fi = new FileInfo(path);
                            if (fi.Exists)
                                fi.Delete();
                        }
                    }
                }
                stokKarti.Resim = fileName;
                stokKarti.KayitYapanAdSoyad = kul.Isim + " " + kul.Soyisim;
                db.Entry(stokKarti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stokKarti);
        }

        // GET: StokKartis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokKarti stokKarti = db.stokKartis.Find(id);
            if (stokKarti == null)
            {
                return HttpNotFound();
            }
            return View(stokKarti);
        }

        // POST: StokKartis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StokKarti stokKarti = db.stokKartis.Find(id);
            db.stokKartis.Remove(stokKarti);
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
