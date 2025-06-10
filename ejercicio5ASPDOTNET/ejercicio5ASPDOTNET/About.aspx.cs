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
        protected void BtnCrearImp_Click(object sender, EventArgs e)
        {
            int idImpresora = Convert.ToInt32(Request.QueryString["id"]);
            byte[] imagenBytes = null;

            if (FileUpload1.HasFile)
            {
                imagenBytes = FileUpload1.FileBytes;
            }
            else
            {
                SqlCommand cmdGetImage = new SqlCommand("SELECT Imagen FROM Impresoras WHERE ImpresoraID = @ImpresoraID", this.coneccion);
                cmdGetImage.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = idImpresora;

                try
                {
                    this.coneccion.Open();
                    object imagenActual = cmdGetImage.ExecuteScalar();
                    this.coneccion.Close();

                    if (imagenActual != DBNull.Value && imagenActual != null)
                    {
                        imagenBytes = (byte[])imagenActual;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener la imagen actual: " + ex.Message);
                }
            }

            SqlCommand cmd = new SqlCommand("EditarImpresora", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = idImpresora;
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

                // Mostrar alerta de éxito con SweetAlert2 antes de la redirección
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta",
                    "Swal.fire({title: '¡Éxito!', text: 'Impresora actualizada correctamente.', icon: 'success'}).then(() => { window.location = 'Default.aspx'; });",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la impresora: " + ex.Message);
            }
        }



    }
}