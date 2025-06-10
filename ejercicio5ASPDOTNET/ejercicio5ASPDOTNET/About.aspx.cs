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
    public partial class About : Page
    {
        private string cs;
        SqlConnection coneccion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cs = ConfigurationManager.ConnectionStrings["sc"].ConnectionString;
            this.coneccion = new SqlConnection(cs);

            if (!IsPostBack)
            {
                string operacion = Request.QueryString["op"];
                string idImpresora = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(operacion))
                {


                    if (operacion.Equals("create"))
                    {
                        BtnCrearImp.Visible = true;
                    }
                    else if (operacion.Equals("update"))
                    {
                        BtnCrearImp.Visible = true;

                        CargarDatosImpresora(Convert.ToInt32(idImpresora));
                    }
                }
            }
        }





        private void CargarDatosImpresora(int idImpresora)
        {
            SqlCommand cmd = new SqlCommand("VerImpresoraPorID", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = idImpresora;

            try
            {
                this.coneccion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txbMarca.Text = reader["Marca"].ToString();
                    txbModelo.Text = reader["Modelo"].ToString();
                    txbUbicacion.Text = reader["Ubicacion"].ToString();
                    txbFechaAdquisicion.Text = Convert.ToDateTime(reader["FechaAdquisicion"]).ToString("yyyy-MM-dd");

                    // Mostrar imagen si existe
                    if (reader["Imagen"] != DBNull.Value)
                    {
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string base64String = Convert.ToBase64String(imagenBytes);
                        imgPreview.ImageUrl = "data:image/png;base64," + base64String;
                    }
                }
                reader.Close();
                this.coneccion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar datos de la impresora: " + ex.Message);
            }
        }
    }
}