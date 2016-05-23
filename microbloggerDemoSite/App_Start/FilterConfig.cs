using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using microbloggerDemoSite.BLL;

namespace microbloggerDemoSite
{
    class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // TODO: find out what this attribute does, may be useful for debugging or cleanliness?
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new AddUserModelToViewBagAttribute());
        }
    }
}
