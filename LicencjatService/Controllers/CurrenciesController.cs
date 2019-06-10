using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using KursyWalutService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KursyWalutService.Controllers
{
    public class CurrenciesController : TableController<Currencies>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            try
            {
                base.Initialize(controllerContext);
                LicencjatContext context = new LicencjatContext();
                DomainManager = new EntityDomainManager<Currencies>(context, Request);
            }
            catch (Exception ex)
            {
                var temp = ex;
            }
        }

        // GET tables/DbTestTable
        [HttpGet]
        public IQueryable<Currencies> GetAllCurrencies()
        {
            return Query();
        }

        public SingleResult<Currencies> GetCurrencies(string id)
        {
            return Lookup(id);
        }

        [HttpPatch]
        public Task<Currencies> PatchCurrencies(string id, Delta<Currencies> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/DbTestTable
        [HttpPost]
        public async Task<IHttpActionResult> PostCurrencies([FromBody] JObject jobj)
        {
            try
            {
                var item = jobj.ToObject<Currencies>();
                var current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task DeleteCurrencies(string id)
        {
            return DeleteAsync(id);
        }
    }
}
