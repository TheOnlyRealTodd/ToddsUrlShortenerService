using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using URLShortener.Data_Transfer_Objects;
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
            if (!HasHttpProtocol(url.OriginalUrl))
            {
                url.OriginalUrl = "http://" + url.OriginalUrl;
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

        public IHttpActionResult Get(int id)
        {
            var url = _context.Urls.SingleOrDefault(u => u.UrlId == id);
            if (url == null)
            {
                NotFound();
            }
            return Ok(url);
        }

        [System.Web.Http.HttpPut]
        public IHttpActionResult Update(int id, UrlDto urlDto)
        {
            var urlInDb = _context.Urls.SingleOrDefault(u => u.UrlId == id);

            if (urlInDb == null)
            {
                return NotFound();
            }
            urlInDb.OriginalUrl = urlDto.OriginalUrl;
            _context.SaveChanges();
            return Ok(urlDto);

        }
        [System.Web.Http.HttpGet]
        
        public IEnumerable<Url> GetUrls()
        {
            var listOfUrls = _context.Urls.ToList();
            return listOfUrls;
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var urlInDb = _context.Urls.SingleOrDefault(u => u.UrlId == id);

            if (urlInDb == null)
            {
                return NotFound();
            }

            _context.Urls.Remove(urlInDb);
            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        public bool HasHttpProtocol(string url)
        {
            url = url.ToLower();
            if (url.Length > 5)
            {
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
