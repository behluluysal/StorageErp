using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace k1835web.Models
{
    public class Depo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Depo Kodu")]
        public string Kodu { get; set; }
        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Depo Adı")]
        public string Ad { get; set; }

        public virtual List<StokKarti> StokKartis { get; set; }
    }
}