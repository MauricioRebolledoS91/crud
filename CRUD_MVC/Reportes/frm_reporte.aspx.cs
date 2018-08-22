using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetodosDeExtension;
using Microsoft.Reporting.WebForms;
using Modelos;

namespace CRUD_MVC.Reportes
{
    public partial class frm_reporte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                byte tipo = Request.QueryString["tipo"].ToByteNotNull();

                switch (tipo)
                {
                    case 1:
                        Reporte();
                        break;
                    case 2:
                        ReporteAgrupado();
                        break;
                    case 3:
                        ReporteAgrupadoColapsado();
                        break;
                }
            }
        }

        private void Reporte()
        {
            try
            {
                List<vConsultaPersona> datos = new List<vConsultaPersona>();

                using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
                {
                    datos = obj.Filter(x => true);
                }
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("vConsultaPersona", datos));

                ReportViewer1.LocalReport.ReportPath = "Reportes/rpt_reporte.rdlc";

                ReportViewer1.Visible = true;
                
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                this.EscribirEnArchivoLog(ex);
            }
        }

        private void ReporteAgrupado()
        {
            try
            {
                List<vConsultaPersona> datos = new List<vConsultaPersona>();

                using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
                {
                    datos = obj.Filter(x => true);
                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("vConsultaPersona", datos));

                ReportViewer1.LocalReport.ReportPath = "Reportes/rpt_reporte_agrupado.rdlc";

                ReportViewer1.Visible = true;

                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                this.EscribirEnArchivoLog(ex);
            }
        }

        private void ReporteAgrupadoColapsado()
        {
            try
            {
                List<vConsultaPersona> datos = new List<vConsultaPersona>();

                using (Repositorio<vConsultaPersona> obj = new Repositorio<vConsultaPersona>())
                {
                    datos = obj.Filter(x => true);
                }

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("vConsultaPersona", datos));

                ReportViewer1.LocalReport.ReportPath = "Reportes/rpt_reporte_agrupado_colapsado.rdlc";

                ReportViewer1.Visible = true;

                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                this.EscribirEnArchivoLog(ex);
            }
        }
    }
}