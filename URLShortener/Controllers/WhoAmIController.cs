using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using URLShortener.ViewModels;

namespace URLShortener.Controllers
{
    public class WhoAmIController : ApiController
    {
        /// <summary>
        /// GET - /api/WhoAmI
        /// This action simply returns the IP address, browser primary language, and operating system of the client computer.
        /// The FigureOutWindowsVersion() method was needed in order to not just display "Windows NT" every time
        /// the action detected a Windows machine.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            string clientOperatingSystemVersion = string.Empty;
            if (HttpContext.Current.Request.Browser.Platform.Contains("WinNT"))
            {
                clientOperatingSystemVersion = FigureOutWindowsVersion();
            }
            else
            {
                clientOperatingSystemVersion = HttpContext.Current.Request.Browser.Platform;
            }
            //var userAgent = HttpContext.Current.Request.UserAgent;
            //var userBrowser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { string.Empty, userAgent } } };
            string[] clientBrowserLanguages = HttpContext.Current.Request.UserLanguages;
            var result = new HeaderData()
            {
                IpAddress = HttpContext.Current.Request.UserHostAddress,
                Language = clientBrowserLanguages[0],
                OperatingSystem = clientOperatingSystemVersion
            };

            return Json(result);
        }

        private string FigureOutWindowsVersion()
        {
            var user = HttpContext.Current.Request.UserAgent;
            if (user.IndexOf("Windows NT 5.1") > 0)
            {
                return "Windows XP";
            }
            else if (user.IndexOf("Windows NT 6.0") > 0)
            {
                return "Windows Vista";
            }
            else if (user.IndexOf("Windows NT 6.1") > 0)
            {
                return "Windows 7";
            }
            else if (user.IndexOf("Windows NT 6.2") > 0)
            {
                return "Windows 8";
            }
            else if (user.IndexOf("Windows NT 6.3") > 0)
            {
                return "Windows 8.1";
            }
            else if (user.IndexOf("Windows NT 10.0") > 0)
            {
                return "Windows 10";
            }
            else
            {
                return "Windows NT - Unknown Edition";
            }
        }
    }
}
