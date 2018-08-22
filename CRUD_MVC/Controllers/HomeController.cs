using Modelos;
using System.Data.Entity.Validation;
using System.Text;
using System.Web.Mvc;
using System.Linq;

namespace CRUD_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public string TiposPersona()
        {
            StringBuilder Resultado = new StringBuilder();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                obj.Excepcion += Obj_Excepcion;
                var datos = obj.Filter(x => true);
                var grupos = datos.GroupBy(x => x.Tipo_Descripcion).ToList();
                Resultado.Append(string.Format("['{0}', '{1}'],", "Tipo", "Cantidad"));
                foreach (var grupo in grupos)
                    Resultado.Append(string.Format("['{0}', {1}],", grupo.FirstOrDefault().Tipo_Descripcion, grupo.Count()));                    
            }            
            return "[" + Resultado.ToString() + "]";
        }

        [ChildActionOnly]
        public string EstatusPersona()
        {
            StringBuilder Resultado = new StringBuilder();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                obj.Excepcion += Obj_Excepcion;
                var datos = obj.Filter(x => true);
                var grupos = datos.GroupBy(x => x.Estatus_Descripcion).ToList();
                Resultado.Append(string.Format("['{0}', '{1}'],", "Tipo", "Cantidad"));
                foreach (var grupo in grupos)
                    Resultado.Append(string.Format("['{0}', {1}],", grupo.FirstOrDefault().Estatus_Descripcion, grupo.Count()));
            }            
            return "[" + Resultado.ToString() + "]";
        }

        private void Obj_Excepcion(object sender, ExceptionEventArgs e)
        {
            if (e.EntityValidationErrors != null)
                throw new DbEntityValidationException(e.Message, e.EntityValidationErrors, e.InnerException) { Source = e.Source };
            else
                throw new System.Exception(e.Message, e.InnerException) { Source = e.Source };
        }
    }
}