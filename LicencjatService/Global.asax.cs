using KursyWalutService.DataMenagment;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KursyWalutService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AddTask("cache", 180);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);          
        }

        private void AddTask(string name, int time)
        {
            CacheItemRemovedCallback OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);

            HttpRuntime.Cache.Insert(name, time, null,
                DateTime.Now.AddSeconds(time), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        private void CacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            PullOnTimer.PeriodicWork();
            AddTask(key, Convert.ToInt32(value));
        }
    }
}


