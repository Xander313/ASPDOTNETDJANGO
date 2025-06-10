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

                // Detectar si hay una solicitud de eliminación en `Mantenimiento.aspx`
                if (Request.QueryString["deleteMantenimientoId"] != null)
                {
                    int idMantenimiento;
                    if (int.TryParse(Request.QueryString["deleteMantenimientoId"], out idMantenimiento))
                    {
                        eliminarMantenimiento(idMantenimiento);
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
        protected void eliminarMantenimiento(int idMantenimiento)
        {
            SqlCommand cmd = new SqlCommand("EliminarMantenimiento", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MantenimientoID", SqlDbType.Int).Value = idMantenimiento;

            try
            {
                this.coneccion.Open();
                cmd.ExecuteNonQuery();
                this.coneccion.Close();

                // Mostrar alerta de éxito con SweetAlert2 después de la eliminación
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta",
                    "Swal.fire({title: '¡Éxito!', text: 'Mantenimiento eliminado correctamente.', icon: 'success'}).then(() => { window.location = 'Mantenimieto.aspx'; });",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el mantenimiento: " + ex.Message);
            }
        }


        protected void BtnCreateMantenimiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AgregarMantenimiento.aspx");
        }

        protected void BtnUpdateMantenimiento_Click(object sender, EventArgs e)
        {
            Button consulta = (Button)sender;
            GridViewRow seleccionRow = (GridViewRow)consulta.NamingContainer;
            int idMantenimiento = Convert.ToInt32(seleccionRow.Cells[0].Text);

            Response.Redirect($"~/EditarMantenimiento.aspx?id={idMantenimiento}");
        }

        protected void BtnDeleteMantenimiento_Click(object sender, EventArgs e)
        {

        }
    }

}