using k1835web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace k1835web.Controllers
{
    public class IslemController : Controller
    {
        private OurDbContext db = new OurDbContext();
        // GET: Islem
        public ActionResult Index()
        {
             

            return View();
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