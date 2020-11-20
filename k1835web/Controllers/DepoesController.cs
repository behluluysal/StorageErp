using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using k1835web.Models;
using k1835web.Security;

namespace k1835web.Controllers
{
    public class DepoesController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: Depoes
        public ActionResult Index()
        {
            return View(db.depos.ToList());
        }

        // GET: Depoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depo depo = db.depos.Find(id);
            if (depo == null)
            {
                return HttpNotFound();
            }
            return View(depo);
        }

        // GET: Depoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Depoes/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Kodu,Ad")] Depo depo)
        {
            if (ModelState.IsValid)
            {
                db.depos.Add(depo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depo);
        }

        // GET: Depoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depo depo = db.depos.Find(id);
            if (depo == null)
            {
                return HttpNotFound();
            }
            return View(depo);
        }

        public ActionResult Islem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depo depo = db.depos.Find(id);
            
            if (depo == null)
            {
                return HttpNotFound();
            }
            ViewBag.depoAdi = depo.Ad + " (" + depo.Kodu + ")";
            return View(db.stokKartis.Where(x=>x.Depo.Id == id).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Islem(string id, string Tip, string Miktar,string Fis)
        {
            int idd=int.Parse(id);
            StokKarti edit = db.stokKartis.Where(x => x.Id == idd).FirstOrDefault();
            Depo depo = db.depos.Where(x => x.Id == edit.Depo.Id).FirstOrDefault();
            ViewBag.depoAdi = depo.Ad + " (" + depo.Kodu + ")";
            int depoid = edit.Depo.Id;
            if (edit.Miktar < int.Parse(Miktar) && Tip =="cikis")
            {
                ViewBag.hata = "miktarFazla";
                return View(db.stokKartis.Where(x => x.Depo.Id == depoid).ToList());
            }
            Kullanici kullanici = db.Admins.Where(x => x.Kullaniciadi == SessionPersister.Username).FirstOrDefault();
            
            DepoGirisCikis islem = new DepoGirisCikis
            {
                Kullanici = kullanici,
                Miktar = long.Parse(Miktar),
                Stok = edit,
                Tarih = DateTime.Now,
                Tip = Tip,
                FisNo = Fis
            };
            db.depoGirisCikis.Add(islem);
            db.SaveChanges();
            if (Tip == "giris")
            {
                edit.Miktar += int.Parse(Miktar);
            }
            else
            {
                edit.Miktar -= int.Parse(Miktar);
            }
            db.SaveChanges();
            return View(db.stokKartis.Where(x => x.Depo.Id == depoid).ToList());
        }

        // POST: Depoes/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Kodu,Ad")] Depo depo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depo);
        }

        // GET: Depoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depo depo = db.depos.Find(id);
            if (depo == null)
            {
                return HttpNotFound();
            }
            return View(depo);
        }

        // POST: Depoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depo depo = db.depos.Find(id);
            db.depos.Remove(depo);
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
