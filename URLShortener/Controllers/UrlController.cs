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
using URLShortener.Binding_Models;
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
        public IHttpActionResult Create(UrlBindingModel urlBindingModel)
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (!ModelState.IsValid)
            {
                BadRequest();
            }
            if (!HasHttpProtocol(urlBindingModel.OriginalUrl))
            {
                urlBindingModel.OriginalUrl = "http://" + urlBindingModel.OriginalUrl;
            }
            int lastEntryId = (from u in _context.Urls
                           orderby u.UrlId descending
                           select u.UrlId).FirstOrDefault();
            if (lastEntryId == null)
            {
                return InternalServerError();
            }
            int newId = lastEntryId + 1;
            var newUrl = new Url()
            {
                OriginalUrl = urlBindingModel.OriginalUrl,
                OurUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/" + newId
            };
            

            _context.Urls.Add(newUrl);
            try //Try to save changes to DB, catch any exceptions. If succcessful, return the new short URL.
            {
                _context.SaveChanges();
                return Created(new Uri(newUrl.OurUrl), newUrl);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
            try
            {
                _context.SaveChanges();
                return Ok(urlDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

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
