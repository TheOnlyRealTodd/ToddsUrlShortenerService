using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URLShortener.Models
{
    public class Url
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; }
        public string OurUrl { get; set; }
    }
}