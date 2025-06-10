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
    public partial class EditarMantenimiento : System.Web.UI.Page
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

                if (Request.QueryString["id"] != null)
                {
                    int mantenimientoID;
                    if (int.TryParse(Request.QueryString["id"], out mantenimientoID))
                    {
                        CargarDatosMantenimiento(mantenimientoID);
                    }
                }
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


        private void CargarDatosMantenimiento(int mantenimientoID)
        {
            SqlCommand cmd = new SqlCommand("VerMantenimientoPorID", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MantenimientoID", SqlDbType.Int).Value = mantenimientoID;

            try
            {
                this.coneccion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ddlImpresoras.SelectedValue = reader["ImpresoraID"].ToString();
                    txbFechaMantenimiento.Text = Convert.ToDateTime(reader["FechaMantenimiento"]).ToString("yyyy-MM-dd");
                    txbTecnico.Text = reader["Tecnico"].ToString();
                    txbDescripcion.Text = reader["Descripcion"].ToString();

                    // Mostrar PDF actual si existe
                    if (reader["InformePDF"] != DBNull.Value)
                    {
                        byte[] pdfBytes = (byte[])reader["InformePDF"];
                        string base64String = Convert.ToBase64String(pdfBytes);
                        iframePDF.Attributes["src"] = "data:application/pdf;base64," + base64String;
                        panelPDF.Visible = true;
                    }
                    else
                    {
                        panelPDF.Visible = false;
                    }
                }

                reader.Close();
                this.coneccion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar mantenimiento: " + ex.Message);
            }
        }

        protected void BtnCrearMantenimiento_Click(object sender, EventArgs e)
        {
            // Obtener el ID del mantenimiento desde la URL
            int mantenimientoID = Convert.ToInt32(Request.QueryString["id"]);
            byte[] pdfBytes = null;

            // Verificar si el usuario subió un nuevo PDF
            if (FileUploadPDF.HasFile)
            {
                pdfBytes = FileUploadPDF.FileBytes;
            }
            else
            {
                // Obtener el PDF actual si no se sube uno nuevo
                SqlCommand cmdGetPDF = new SqlCommand("SELECT InformePDF FROM Mantenimientos WHERE MantenimientoID = @MantenimientoID", this.coneccion);
                cmdGetPDF.Parameters.Add("@MantenimientoID", SqlDbType.Int).Value = mantenimientoID;

                try
                {
                    this.coneccion.Open();
                    object pdfActual = cmdGetPDF.ExecuteScalar();
                    this.coneccion.Close();

                    if (pdfActual != DBNull.Value && pdfActual != null)
                    {
                        pdfBytes = (byte[])pdfActual;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener el informe PDF actual: " + ex.Message);
                }
            }

            // Crear el comando para actualizar el mantenimiento
            SqlCommand cmd = new SqlCommand("EditarMantenimiento", this.coneccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MantenimientoID", SqlDbType.Int).Value = mantenimientoID;
            cmd.Parameters.Add("@ImpresoraID", SqlDbType.Int).Value = Convert.ToInt32(ddlImpresoras.SelectedValue);
            cmd.Parameters.Add("@FechaMantenimiento", SqlDbType.Date).Value = Convert.ToDateTime(txbFechaMantenimiento.Text);
            cmd.Parameters.Add("@Tecnico", SqlDbType.NVarChar).Value = txbTecnico.Text.Trim();
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = txbDescripcion.Text.Trim();

            // Manejar el informe en PDF correctamente
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

                // Mostrar alerta de éxito con SweetAlert2 después de la edición
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta",
                    "Swal.fire({title: '¡Éxito!', text: 'Mantenimiento actualizado correctamente.', icon: 'success'}).then(() => { window.location = 'Mantenimieto.aspx'; });",
                    true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar mantenimiento: " + ex.Message);
            }
        }

    }
}