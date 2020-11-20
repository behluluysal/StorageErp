using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using k1835web.Models;
using k1835web.Security;

namespace k1835web.Controllers
{
    public class AdminsController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Admins.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.


        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Admins.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }


        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Admins.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kullanici kullanici = db.Admins.Find(id);
            db.Admins.Remove(kullanici);
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

        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult panel()
        {
            if (Session["lang"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["lang"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["lang"].ToString());
            }

            return View(db.depoGirisCikis.ToList());
        }
        public ActionResult Login()
        {
            if (Request.Cookies["cerezim"] != null)
            {
                string login;
                HttpCookie kayitlicerez = Request.Cookies["cerezim"];
                login = LoginSecure(kayitlicerez.Values["kullaniciadi"], kayitlicerez.Values["sifre"], "on");
                if (login == "su")
                {
                    return RedirectToAction("panel", "Admins");
                }
                else if (login == "admin")
                {
                    return RedirectToAction("panel", "Admins");
                }
                else
                {
                    return RedirectToAction("Login", "Admins");
                }
            }
            if (Session["lang"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["lang"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["lang"].ToString());
            }
            if (Session["modtype"] != null)
                return RedirectToAction("panel", "Admins");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Create([Bind(Include = "Id,Kullaniciadi,Isim,Soyisim,Sifre")] Kullanici admin)
        {
            if (ModelState.IsValid)
            {
                Kullanici control = db.Admins.AsNoTracking().Where(x => x.Kullaniciadi == admin.Kullaniciadi).FirstOrDefault();
                if (control != null)
                {
                    TempData["dublicate"] = "match";
                    return RedirectToAction("Create");
                }
                admin.Roletr = "superadmin";
                admin.Sifre = MD5Sifrele(admin.Sifre);
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }
        [HttpPost]
        [CustomAuthorize(Roles = "superadmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Kullaniciadi,Isim,Soyisim,Sifre")] Kullanici admin)
        {
            if (ModelState.IsValid)
            {
                Kullanici control = db.Admins.AsNoTracking().Where(x => x.Kullaniciadi == admin.Kullaniciadi).FirstOrDefault();
                if (control != null)
                {
                    if (control.Id != admin.Id)
                    {
                        TempData["dublicate"] = "match";
                        return RedirectToAction("Edit", new { admin.Id });
                    }

                }
                admin.Sifre = MD5Sifrele(admin.Sifre);
                admin.Roletr = "superadmin";
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name, string password, string rememberMe)
        {
            string login;
            password = MD5Sifrele(password);
            login = LoginSecure(name, password, rememberMe);
            if (login == "su")
            {
                return RedirectToAction("panel", "Admins");
            }
            else if (login == "admin")
            {
                return RedirectToAction("panel", "Admins");
            }
            else
            {
                return RedirectToAction("Login", "Admins");
            }
        }
        protected string LoginSecure(string name, string password, string rememberMe)
        {
            if (Session["lang"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["lang"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["lang"].ToString());
            }
            using (OurDbContext db = new OurDbContext())
            {
                var usr = db.Admins.Where(u => u.Kullaniciadi == name && u.Sifre == password).FirstOrDefault();

                if (usr != null)
                {
                    Session.Timeout = 60;
                    SessionPersister.Username = name;
                    if (rememberMe == "on")
                    {
                        HttpCookie cerez = new HttpCookie("cerezim");
                        cerez.Values.Add("sifre", password);
                        cerez.Values.Add("yetkiid", usr.Roletr);
                        cerez.Values.Add("kullaniciadi", usr.Kullaniciadi);
                        cerez.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cerez);
                    }

                    if (usr.Roletr == "superadmin")
                    {
                        Session["modtype"] = "Yönetici";
                        return "su";
                    }
                    else
                    {
                        Session["modtype"] = "Yardımcı";
                        return "admin";
                    }
                }
                else
                    return "error";
            }
        }



        public ActionResult Logout()
        {
            SessionPersister.Username = string.Empty;
            Session.Abandon();
            Session["modtype"] = null;
            Session["activeSeason"] = null;
            if (Request.Cookies["cerezim"] != null)
            {
                Response.Cookies["cerezim"].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Login", "Admins");
        }

        public static string MD5Sifrele(string sifrelenecekMetin)
        {

            // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            //dizinin hash'ini hesaplattık.
            dizi = md5.ComputeHash(dizi);
            //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
            StringBuilder sb = new StringBuilder();
            //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürdük.
            return sb.ToString();
        }
    }
}
