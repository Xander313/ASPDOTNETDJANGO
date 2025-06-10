using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ejercicio5ASPDOTNET
{
    public partial class Contact : Page
    {
        private string cs;
        SqlConnection coneccion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cs = ConfigurationManager.ConnectionStrings["sc"].ConnectionString;
            this.coneccion = new SqlConnection(cs);

        }

        protected void BtnCrearImp_Click(object sender, EventArgs e)
        {
            byte[] imagenBytes = null;
            if (fuImagen.HasFile)
            {
                imagenBytes = fuImagen.FileBytes;
            }

            SqlCommand cmd = new SqlCommand("AgregarImpresora", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Marca", SqlDbType.NVarChar).Value = txbMarca.Text.Trim();
            cmd.Parameters.Add("@Modelo", SqlDbType.NVarChar).Value = txbModelo.Text.Trim();
            cmd.Parameters.Add("@Ubicacion", SqlDbType.NVarChar).Value = txbUbicacion.Text.Trim();
            cmd.Parameters.Add("@FechaAdquisicion", SqlDbType.Date).Value = Convert.ToDateTime(txbFechaAdquisicion.Text);
            SqlParameter paramImagen = new SqlParameter("@Imagen", SqlDbType.VarBinary);
            if (imagenBytes != null)
            {
                paramImagen.Value = imagenBytes;
            }
            else
            {
                paramImagen.Value = DBNull.Value;
            }
            cmd.Parameters.Add(paramImagen);
            try
            {
                this.coneccion.Open();
                cmd.ExecuteNonQuery();
                this.coneccion.Close();
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar impresora: " + ex.Message);
            }

        }
    }
}