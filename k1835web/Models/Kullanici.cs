using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace k1835web.Models
{
    [Table("Kullanici")]
    public class Kullanici
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage ="Bu alan bos gecilemez")]
        [Display(Name ="Kullanici Adi")]
        public string Kullaniciadi { get; set; }

        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "İsim")]
        public string Isim { get; set; }

        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Soyisim")]
        public string Soyisim { get; set; }


        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Sifre")]
        public string Sifre { get; set; }

        public string Roletr { get; set; }
        public virtual List<DepoGirisCikis> DepoGirisCikis { get; set; }

    }
}