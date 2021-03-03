using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Programmesana_Sanija_Airita
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           // FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {     if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Programmesana_Sanija_Airita.Controllers.DataAccess.UsersRepository ur = new Controllers.DataAccess.UsersRepository();
                    var userFromDb = ur.GetUserByUsername(User.Identity.Name); 

                    var rolesOfUser = userFromDb.Roles;

                    GenericPrincipal gp = new GenericPrincipal(User.Identity, rolesOfUser.Select(x => x.Title).ToArray());

                    Context.User = gp;

                }
            }
        }

    }
}
