using System;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRUD_MVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            string error = exc.Message.Replace("'", "");
            if (exc.InnerException != null)
            {
                error += " InnerException.Message: " + exc.InnerException.Message.Replace("'", "");
                if (exc.InnerException.InnerException != null)
                    error += " InnerException.InnerException.Message: " + exc.InnerException.InnerException.Message.Replace("'", "");
            }                

            string usuario = System.Security.Principal.WindowsIdentity.GetCurrent().Name;            

            if (exc.GetType() == typeof(HttpException))
            {
                //Manejo de la HttpException                
                Response.Write(string.Format("<Script Language='javascript'>alert('{0}'); window.location.href = 'index'</script>", error + " Usuario: " + usuario));
            }
            else if (exc.GetType() == typeof(DbEntityValidationException))
            {
                //Manejo de la DbEntityValidationException                 
                Response.Write(string.Format("<Script Language='javascript'>alert('{0}'); window.location.href = 'index'</script>", error + " Usuario: " + usuario));
            }
            else
            {
                //Manejo de la Exception general                
                Response.Write(string.Format("<Script Language='javascript'>alert('{0}'); window.location.href = 'index'</script>", error + " Usuario: " + usuario));
            }

            // Limpiar el error de
            Server.ClearError();
        }
    }
}
