using System.Web.Http;
using System.Web.Http.Tracing;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace KursyWalutService.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    public class ValuesController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            MobileAppSettingsDictionary settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();
            ITraceWriter traceWriter = this.Configuration.Services.GetTraceWriter();

            string host = settings.HostName ?? "localhost";
            string greeting = "Hello from " + host;

            return new HttpResponseMessage { Content = new StringContent(greeting) };
            //normalnie przekazuje json object ktory trzeba deserializowac
            //mozna tez zrobic tak jak tu i mam funkcje do czytania contentu z http
        }

        // POST api/values
        public HttpResponseMessage Post()
        {
            var str = "dodano";
            //var mess = new HttpResponseMessage() { Content = JsonConvert.SerializeObject. }
            return Request.CreateResponse(HttpStatusCode.OK,str);
        }
    }
}
