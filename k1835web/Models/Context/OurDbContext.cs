using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using k1835web.Models;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

namespace k1835web.Models
{
    public class OurDbContext : DbContext
    {

        public DbSet<Kullanici> Admins { get; set; }
        public DbSet<Depo> depos { get; set; }
        public DbSet<StokKarti> stokKartis { get; set; }
        public DbSet<DepoGirisCikis> depoGirisCikis { get; set; }

        public OurDbContext()
        {
            //this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new VeriTabaniOlusturucu());
        }

        
    }

    public class VeriTabaniOlusturucu : CreateDatabaseIfNotExists<OurDbContext>
    {
        protected override void Seed(OurDbContext context)
        {
            Kullanici usr = new Kullanici();
            usr.Kullaniciadi = "su";
            usr.Sifre = MD5Sifrele("123");
            usr.Roletr = "superadmin";
            usr.Isim = "Behlul";
            usr.Soyisim = "UYSAL";
            context.Admins.Add(usr);
            context.SaveChanges();

            Depo dep = new Depo();
            dep.Kodu = "1";
            dep.Ad = "Depo1";
            context.depos.Add(dep);
            context.SaveChanges();

            StokKarti stok = new StokKarti();
            stok.KayitTarihi = DateTime.Now;
            stok.KayitYapanAdSoyad = "Behlul UYSAL";
            stok.Kodu = "S1";
            stok.Resim = "location1";
            stok.Kdv = 0.34;
            stok.Adi = "stok1";
            stok.Aciklama = "aciklama1";
            stok.Fiyat = 100;
            stok.Depo = dep;
            stok.Miktar = 100;
            context.stokKartis.Add(stok);
            context.SaveChanges();

            DepoGirisCikis islem = new DepoGirisCikis();
            islem.FisNo = "fis1";
            islem.Tarih = DateTime.Now;
            islem.Tip = "giris";
            islem.Miktar = 100;
            islem.Stok = stok;
            islem.Kullanici = usr;
            context.depoGirisCikis.Add(islem);
            context.SaveChanges();


            List<Kullanici> admins = context.Admins.ToList();
            List<Depo> depos = context.depos.ToList();
            List<StokKarti> stokKartis = context.stokKartis.ToList();
            List<DepoGirisCikis> depoGirisCikis = context.depoGirisCikis.ToList();
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

