using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ejercicio5ASPDOTNET
{
    public partial class AgregarMantenimiento : System.Web.UI.Page
    {
        private string cs;
        SqlConnection coneccion;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cs = ConfigurationManager.ConnectionStrings["sc"].ConnectionString;
            this.coneccion = new SqlConnection(cs);

            if (!IsPostBack)
            {
                CargarImpresoras();
            }
        }

        private void CargarImpresoras()
        {
            SqlCommand cmd = new SqlCommand("VerTodasImpresoras", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                this.coneccion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListItem item = new ListItem($"{reader["Marca"]} - {reader["Modelo"]}", reader["ImpresoraID"].ToString());
                    ddlImpresoras.Items.Add(item);
                }

                reader.Close();
                this.coneccion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar impresoras: " + ex.Message);
            }
        }




        protected void BtnCrearMantenimiento_Click(object sender, EventArgs e)
        {
            byte[] pdfBytes = null;

            // Verificar si el usuario subió un PDF
            if (FileUploadPDF.HasFile)
            {
                pdfBytes = FileUploadPDF.FileBytes;
            }

            int impresoraID = Convert.ToInt32(ddlImpresoras.SelectedValue);

            SqlCommand cmd = new SqlCommand("AgregarMantenimiento", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = impresoraID;
            cmd.Parameters.Add("@FechaMantenimiento", SqlDbType.Date).Value = Convert.ToDateTime(txbFechaMantenimiento.Text);
            cmd.Parameters.Add("@Tecnico", SqlDbType.NVarChar).Value = txbTecnico.Text.Trim();
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = txbDescripcion.Text.Trim();

            SqlParameter paramPDF = new SqlParameter("@InformePDF", SqlDbType.VarBinary);
            if (pdfBytes != null)
            {
                paramPDF.Value = pdfBytes;
            }
            else
            {
                paramPDF.Value = DBNull.Value;
            }
            cmd.Parameters.Add(paramPDF);

            try
            {
                this.coneccion.Open();
                cmd.ExecuteNonQuery();
                this.coneccion.Close();

                ScriptManager.RegisterStartupScript(this, GetType(), "alerta",
                    "Swal.fire({title: '¡Éxito!', text: 'Mantenimiento registrado correctamente.', icon: 'success'}).then(() => { window.location = 'Mantenimieto.aspx'; });",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar mantenimiento: " + ex.Message);
            }
        }





    }
}