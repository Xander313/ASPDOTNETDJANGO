using System;
using System.Data.SqlClient;
using System.Web;

namespace ejercicio5ASPDOTNET
{
    public class HandlerImagen : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // Obtener el ID de la impresora desde la URL
            string idImpresora = context.Request.QueryString["ImpresoraID"];
            if (!string.IsNullOrEmpty(idImpresora))
            {
                SqlConnection conn = new SqlConnection("Server=DESKTOP-PDQQFC6\\SQLEXPRESS;Database=ejercicio5dotnet;Trusted_Connection=True;");
                SqlCommand cmd = new SqlCommand("SELECT Imagen FROM Impresoras WHERE ImpresoraID = @ImpresoraID", conn);
                cmd.Parameters.AddWithValue("@ImpresoraID", idImpresora);

                try
                {
                    conn.Open();
                    object imagenData = cmd.ExecuteScalar();
                    conn.Close();

                    if (imagenData != DBNull.Value && imagenData != null)
                    {
                        byte[] imagenBytes = (byte[])imagenData;
                        context.Response.ContentType = "image/png"; // Cambia el formato según sea necesario
                        context.Response.BinaryWrite(imagenBytes);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error al cargar la imagen: " + ex.Message);
                }
            }
        }

        public bool IsReusable => false;
    }
}
