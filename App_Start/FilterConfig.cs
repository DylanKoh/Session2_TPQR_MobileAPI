using System.Web;
using System.Web.Mvc;

namespace Session2_TPQR_MobileAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
