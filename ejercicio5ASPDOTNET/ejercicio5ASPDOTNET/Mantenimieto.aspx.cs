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

    public partial class Mantenimieto : System.Web.UI.Page
    {
        private string cs;
        SqlConnection coneccion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cs = ConfigurationManager.ConnectionStrings["sc"].ConnectionString;
            this.coneccion = new SqlConnection(cs);

        }


        private void BindDataImpresoras()
        {
            SqlCommand cmd = new SqlCommand("VerTodasImpresoras", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                this.coneccion.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                this.coneccion.Close();

                // Convertir la imagen binaria en Base64
                dt.Columns.Add("ImagenBase64", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Imagen"] != DBNull.Value)
                    {
                        byte[] imagenBytes = (byte[])row["Imagen"];
                        string base64String = Convert.ToBase64String(imagenBytes);
                        row["ImagenBase64"] = "data:image/png;base64," + base64String;
                    }
                }

                DGVMantenimientos.DataSource = dt;
                DGVMantenimientos.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en BindDataImpresoras: " + ex.Message);
            }
        }


        // Método para eliminar mantenimiento desde el servidor
        protected void eliminarImpresora(int idImpresora)
        {
            SqlCommand cmd = new SqlCommand("EliminarImpresora", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = idImpresora;

            try
            {
                this.coneccion.Open();
                cmd.ExecuteNonQuery();
                this.coneccion.Close();

                // Mostrar alerta de éxito con SweetAlert2 después de la eliminación
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta",
                    "Swal.fire({title: '¡Éxito!', text: 'Impresora eliminada correctamente.', icon: 'success'}).then(() => { window.location = 'Default.aspx'; });",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la impresora: " + ex.Message);
            }
        }
    }

}