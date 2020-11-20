using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace k1835web.Models
{
    public class DepoGirisCikis
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "İşlem Tarihi")]
        public DateTime Tarih { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Fiş Numarası")]
        public string FisNo { get; set; }
        [Display(Name = "Miktar")]
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        public long Miktar { get; set; }
        [Display(Name = "İşlem Türü")]
        public string Tip { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual StokKarti Stok { get; set; }
    }
}