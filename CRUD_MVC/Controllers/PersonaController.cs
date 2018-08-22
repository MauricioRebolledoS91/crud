using MetodosDeExtension;
using Modelos;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;
using System.Web.Mvc;
using System.Linq;

namespace CRUD_MVC.Controllers
{
    public class PersonaController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            using (Repositorio<Persona_Tipo> obj = new Repositorio<Persona_Tipo>())
            {
                ViewBag.Persona_Tipo = obj.Filter(x => true);
            }
            using (Repositorio<Estatu> obj = new Repositorio<Estatu>())
            {                
                ViewBag.Estatus = obj.Filter(x => true);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Persona persona)
        {
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Create(persona);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public ActionResult CreateConAjax(Persona persona)
        {
            bool result = false;
            string mensaje = "Error al crea el registro";
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                if (obj.Create(persona) != null)
                {
                    result = true;
                    mensaje = "Registro creado!";
                }
            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Listar()
        {
            //Enviamos el modelo vacío para llenarlo con ambas funciones
            List<vConsultaPersona> datos = new List<vConsultaPersona>();
            return View(datos);
        }

        public ActionResult ListarSinAjax()
        {
            List<vConsultaPersona> datos = new List<vConsultaPersona>();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                datos = obj.Filter(x => true);
            }            
            return View("Listar", datos);
        }

        [HttpGet]
        public ActionResult ListarAjax()
        {            
            List<vConsultaPersona> datos = new List<vConsultaPersona>();
            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                obj.Excepcion += Obj_Excepcion;
                datos = obj.Filter(x => true);
            }            
            return Json(new { data = datos }, JsonRequestBehavior.AllowGet);
        }

        private void Obj_Excepcion(object sender, ExceptionEventArgs e)
        {
            if (e.EntityValidationErrors != null)
                throw new DbEntityValidationException(e.Message, e.EntityValidationErrors, e.InnerException) { Source = e.Source };
            else
                throw new System.Exception(e.Message, e.InnerException) { Source = e.Source };
        }
        
        [HttpPost]
        public ActionResult Actualizar(int id, string PropertyName, string value)
        {
            bool status = false; string mensaje = "Valor no establecido";
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                switch (PropertyName)
                {
                    case "Fecha_Nacimiento":
                        obj.Update(x => x.Id == id, PropertyName, value.ToDate());
                        break;
                    case "Balance":
                        obj.Update(x => x.Id == id, PropertyName, value.ToDecimal());
                        break;
                    case "Id_Persona_Tipo":
                        obj.Update(x => x.Id == id, PropertyName, value.ToIntNotNull());
                        break;
                    case "Id_Estatus":
                        obj.Update(x => x.Id == id, PropertyName, value.ToByteNotNull());
                        break;
                    default:
                        obj.Update(x => x.Id == id, PropertyName, value);
                        break;
                }
                status = true;
                mensaje = "Valor establecido";
            }

            using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
            {
                switch (PropertyName)
                {
                    case "Id_Persona_Tipo":
                        value = obj.Retrieve(x => x.Id == id).Tipo_Descripcion;
                        break;
                    case "Id_Estatus":
                        value = obj.Retrieve(x => x.Id == id).Estatus_Descripcion;
                        break;
                }                
            }            
            return Json(new { value = value, status = status, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            using (Repositorio<Persona_Tipo> obj = new Repositorio<Persona_Tipo>())
            {
                ViewBag.Persona_Tipo = obj.Filter(x => true);
            }
            using (Repositorio<Estatu> obj = new Repositorio<Estatu>())
            {
                ViewBag.Estatus = obj.Filter(x => true);
            }
            if (id != null)
            {
                using (Repositorio<Persona> obj = new Repositorio<Persona>())
                {
                    var modelo = obj.Retrieve(x => x.Id == id);
                    if (modelo != null)
                    {
                        return View(modelo);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Editar(Persona persona)
        {
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Update(persona);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditarAjax(Persona persona)
        {
            bool result = false;
            string mensaje = "Error al actualizar el registro";
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Update(persona);
                result = true;
                mensaje = "Registro actualizado!";
            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //Para el uso de jeditable
        public string Tipos_PersonaJSON()
        {
            StringBuilder sb = new StringBuilder();
            using (Repositorio<Persona_Tipo> obj = new Repositorio<Persona_Tipo>())
            {
                obj.Excepcion += Obj_Excepcion;
                var datos = obj.Filter(x => true);
                foreach (var item in datos)
                    sb.Append(string.Format("'{0}':'{1}',", item.Id, item.Descripcion));
            }
            return "{" + sb.ToString() + "}";
        }

        //Para el uso de jeditable
        public string EstatusJSON()
        {
            StringBuilder sb = new StringBuilder();
            using (Repositorio<Estatu> obj = new Repositorio<Estatu>())
            {
                obj.Excepcion += Obj_Excepcion;
                var datos = obj.Filter(x => true);
                foreach (var item in datos)
                    sb.Append(string.Format("'{0}':'{1}',", item.Id, item.Descripcion));
            }
            return "{" + sb.ToString() + "}";
        }

        [HttpGet]
        public ActionResult Borrar(int? id)
        {
            if (id != null)
            {
                using (Repositorio<Persona> obj = new Repositorio<Persona>())
                {
                    obj.Excepcion += Obj_Excepcion;
                    var modelo = obj.Retrieve(x => x.Id == id);
                    if (modelo != null)
                    {
                        return View(modelo);
                    }
                }
            }            
            return View();
        }

        [HttpPost]
        public ActionResult Borrar(Persona persona)
        {
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Delete(persona);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult BorrarAjax(Persona persona)
        {
            bool result = false;
            string mensaje = "Error al borrar registro";
            using (Repositorio<Persona> obj = new Repositorio<Persona>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Delete(persona);
                result = true;
                mensaje = "Registro borrado!";
            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //Reportes
        public ActionResult Reporte()
        {
            return Redirect("~/Reportes/frm_reporte.aspx?tipo=1");
        }

        public ActionResult ReporteAgrupado()
        {
            return Redirect("~/Reportes/frm_reporte.aspx?tipo=2");
        }

        public ActionResult ReporteAgrupadoColapsado()
        {
            return Redirect("~/Reportes/frm_reporte.aspx?tipo=3");
        }

        //Manejo de los gráficos
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
                    Resultado.Append(string.Format("['{0}', {1}],",
                        grupo.FirstOrDefault().Tipo_Descripcion, grupo.Count()));
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
                    Resultado.Append(string.Format("['{0}', {1}],",
                        grupo.FirstOrDefault().Estatus_Descripcion, grupo.Count()));
            }
            return "[" + Resultado.ToString() + "]";
        }
    }
}