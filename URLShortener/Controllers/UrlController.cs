using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class UrlController : ApiController
    {
        private ApplicationDbContext _context;

        public UrlController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult Create(Url url)
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (!ModelState.IsValid)
            {
                BadRequest();
            }
            int lastEntryId = (from u in _context.Urls
                           orderby u.UrlId descending
                           select u.UrlId).FirstOrDefault();
            if (lastEntryId == null)
            {
                return InternalServerError();
            }
            url.UrlId = lastEntryId + 1;
            url.OurUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/" + url.UrlId;

            _context.Urls.Add(url);
            _context.SaveChanges();


            
            return Ok();
        }

        public IHttpActionResult GetUrl(int id)
        {
            var url = _context.Urls.SingleOrDefault(u => u.UrlId == id);
            if (url == null)
            {
                NotFound();
            }
            return Ok(url);
        }
        [System.Web.Http.HttpGet]
        
        public IEnumerable<Url> GetUrls()
        {
            var listOfUrls = _context.Urls.ToList();
            return listOfUrls;
        }
    }
}
