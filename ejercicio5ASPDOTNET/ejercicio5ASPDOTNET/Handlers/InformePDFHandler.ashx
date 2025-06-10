<%@ WebHandler Language="C#" Class="InformePDFHandler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public class InformePDFHandler : IHttpHandler
{
    private string cs = System.Configuration.ConfigurationManager.ConnectionStrings["sc"].ConnectionString;

    public void ProcessRequest(HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            int idMantenimiento;
            if (int.TryParse(context.Request.QueryString["id"], out idMantenimiento))
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT InformePDF FROM Mantenimientos WHERE MantenimientoID = @MantenimientoID", connection);
                    cmd.Parameters.Add("@MantenimientoID", SqlDbType.Int).Value = idMantenimiento;

                    try
                    {
                        connection.Open();
                        object informePDF = cmd.ExecuteScalar();
                        connection.Close();

                        if (informePDF != DBNull.Value && informePDF != null)
                        {
                            byte[] pdfBytes = (byte[])informePDF;
                            context.Response.ContentType = "application/pdf";
                            context.Response.BinaryWrite(pdfBytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write("Error al obtener el PDF: " + ex.Message);
                    }
                }
            }
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
