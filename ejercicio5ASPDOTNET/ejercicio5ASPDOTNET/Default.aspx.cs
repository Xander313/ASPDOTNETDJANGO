using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ejercicio5ASPDOTNET
{
    public partial class _Default : Page
    {
        private string cs;
        SqlConnection coneccion;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cs = ConfigurationManager.ConnectionStrings["sc"].ConnectionString;
            this.coneccion = new SqlConnection(cs);
            if (!IsPostBack)
            {
                BindData();

                // Detectar si hay una solicitud de eliminación por query string
                if (Request.QueryString["deleteId"] != null)
                {
                    int idImpresora;
                    if (int.TryParse(Request.QueryString["deleteId"], out idImpresora))
                    {
                        eliminarImpresora(idImpresora);
                    }
                }
            }


        }


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


        private void BindData()
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

                // Crear una nueva columna para almacenar la URL de la imagen en Base64
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

                DGVImpresoras.DataSource = dt;
                DGVImpresoras.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en BindData: " + ex.Message);
            }
        }



        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contact.aspx");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {

            string id;
            Button consulta = (Button)sender;
            GridViewRow seleccionRow = (GridViewRow)consulta.NamingContainer;
            id = seleccionRow.Cells[0].Text;

            Response.Redirect("~/about.aspx?op=update&id=" + id);

        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            Button consulta = (Button)sender;
            GridViewRow seleccionRow = (GridViewRow)consulta.NamingContainer;
            int idImpresora = Convert.ToInt32(seleccionRow.Cells[0].Text);

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