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
            if (!IsPostBack)
            {
                BindDataMantenimientos();

                // Detectar si hay una solicitud de eliminación por query string
                if (Request.QueryString["deleteId"] != null)
                {
                    int idImpresora;
                    if (int.TryParse(Request.QueryString["deleteId"], out idImpresora))
                    {
                        eliminarMantenimieto(idImpresora);
                    }
                }
            }

        }


        private void BindDataMantenimientos()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Mantenimientos", this.coneccion);

            try
            {
                this.coneccion.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                this.coneccion.Close();

                // Crear nueva columna para almacenar el PDF en Base64
                dt.Columns.Add("PDFBase64", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    if (row["InformePDF"] != DBNull.Value)
                    {
                        byte[] pdfBytes = (byte[])row["InformePDF"];
                        string base64String = Convert.ToBase64String(pdfBytes);
                        row["PDFBase64"] = "data:application/pdf;base64," + base64String;
                    }
                }

                DGVMantenimientos.DataSource = dt;
                DGVMantenimientos.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en BindDataMantenimientos: " + ex.Message);
            }
        }



        // Método para eliminar mantenimiento desde el servidor
        protected void eliminarMantenimieto(int idImpresora)
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

        protected void BtnCreateMantenimiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AgregarMantenimiento.aspx");
        }
    }

}