using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class RedirectController : Controller
    {
        private ApplicationDbContext _context;

        public RedirectController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Redirect
        [HttpGet]
        public ActionResult Go(int id)
        {
            var redirectUrl = _context.Urls.SingleOrDefault(u =>  u.UrlId== id);
            if (redirectUrl == null)
            {
                return HttpNotFound();
            }

            return Redirect(redirectUrl.OriginalUrl);
        }
    }
}