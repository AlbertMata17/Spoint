using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SpointLiteVersion.RTPFactura
{
    public partial class cotizar : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand comando;
        SqlDataAdapter adapter;
        SqlParameter param;
        string idVenta;
        string nombreempresa;
        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=DESKTOP-MF01SN4\\SQLANALYSIS;Initial Catalog=spoint;Integrated Security=True");
            if (!IsPostBack)
            {
                renderReport();

            }


        }
        public void renderReport()
        {
            idVenta = Request.QueryString.Get("idventa");

            DataTable dt = cargar(idVenta);
            ReportDataSource rds = new ReportDataSource("DataSet2", dt);
            ReportViewer2.LocalReport.DataSources.Add(rds);
            ReportViewer2.LocalReport.ReportPath = "RTPFactura/Report3.rdlc";
            PageSettings pg = new PageSettings();
            pg.Margins.Left = 0;
            pg.Margins.Right = 40;
            pg.Margins.Top = 50;
            pg.Margins.Bottom = 40;
            this.ReportViewer2.SetPageSettings(pg);

            //parameters
            ReportParameter[] rptParams = new ReportParameter[]
            {
                new ReportParameter("idventa",idVenta.ToString())
        };
            ReportViewer2.LocalReport.Refresh();

        }

        public DataTable cargar(string codigoventa)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection("Data Source=DESKTOP-MF01SN4\\SQLANALYSIS;Initial Catalog=spoint;Integrated Security=True"))
            {

                SqlCommand cmd = new SqlCommand("sp_reporte_cotizacion_back", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idVenta", SqlDbType.Int).Value = idVenta;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;

        }
    }
}