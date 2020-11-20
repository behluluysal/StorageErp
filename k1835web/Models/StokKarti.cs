using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace k1835web.Models
{
    public class StokKarti
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Stok Kodu")]
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        public string Kodu { get; set; }
        [Display(Name = "Stok Adı")]
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        public string Adi { get; set; }
        [Display(Name = "KDV Miktarı")]
        public double Kdv { get; set; }
        [Display(Name = "Stok Fiyatı")]
        public double Fiyat { get; set; }
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }
        [Display(Name = "Resim")]
        public string Resim { get; set; }

        [Display(Name = "Stoğu Kayıt Eden Kullanıcı")]
        public string KayitYapanAdSoyad { get; set; }
        [Display(Name = "Kayıt Tarihi")]
        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime KayitTarihi { get; set; }
        public long Miktar { get; set; }

        public virtual List<DepoGirisCikis> DepoGirisCikis { get; set; }
        public virtual Depo Depo { get; set; }
    }
}