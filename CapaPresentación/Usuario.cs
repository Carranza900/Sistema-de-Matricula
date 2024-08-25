using CapaNegocios;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace CapaPresentación
{
    public partial class Usuario : Form
    {
        public Usuario()
        {
            InitializeComponent();
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(this.dgvData_DataError);

            btnActualizar.Visible = false;
            btnCancelar.Visible = false;
            btnInsertar.Visible = true;
            btnEliminar.Visible = true;
            txtId.Visible = false;
        }

        private void limpiar()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtUsuario.Clear();
            txtClave.Clear();

            cmbRol.SelectedIndex = -1; 
            cmbEstado.SelectedIndex = -1;

            btnActualizar.Visible = false;
            btnInsertar.Visible = true;
            btnCancelar.Visible = false;
            btnEliminar.Visible = false;
            error.Clear();

            dgvData.Columns[0].Visible = false;
            chkSeleccionar.Checked = false;
        }

        private void listar()
        {
            try
            {
                dgvData.DataSource = CN_Usuario.Listar();
                this.formato();
                lblTotal.Text = "Total registros: " + Convert.ToString(dgvData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void formato()
        {
            dgvData.Columns[0].Visible = false; // seleccionar
            dgvData.Columns[1].Visible = false; // id
            dgvData.Columns[2].Width = 100;
            dgvData.Columns[3].Width = 120;
            dgvData.Columns[4].Width = 80;
            dgvData.Columns[5].Visible = false; // Contraseña
            dgvData.Columns[6].Width = 100;
            dgvData.Columns[7].Width = 100;

            try
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells["Estado"].Value != null)
                    {
                        row.Cells["Estado"].Value = Convert.ToInt32(row.Cells["Estado"].Value) == 1 ? "Activo" : "Inactivo";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al formatear el estado: " + ex.Message);
            }
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            this.dgvData.DataError += new DataGridViewDataErrorEventHandler(this.dgvData_DataError);

            cmbRol.Items.Clear();
            cmbRol.Items.Add("Admin");
            cmbRol.Items.Add("Profesor");
            cmbRol.Items.Add("Estudiante");

            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");

            txtId.Visible = false;

            this.listar();
            this.formato();

            limpiar();

            lblTotal.Text = "Total registros: " + dgvData.Rows.Count;
        }
        private void BuscarUsuario()
        {

            try
            {
                string valorBusqueda = txtbuscar.Text;
                dgvData.DataSource = CN_Usuario.Buscar(valorBusqueda);
                this.formato();
                lblTotal.Text = "Total registros: " + Convert.ToString(dgvData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void MensajeOK(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de alumnos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarUsuario();
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtId.Text = dgvData.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? string.Empty;
                    txtNombre.Text = dgvData.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? string.Empty;
                    txtApellido.Text = dgvData.Rows[e.RowIndex].Cells[3].Value?.ToString() ?? string.Empty;
                    txtCorreo.Text = dgvData.Rows[e.RowIndex].Cells[4].Value?.ToString() ?? string.Empty;
                    txtUsuario.Text = dgvData.Rows[e.RowIndex].Cells[5].Value?.ToString() ?? string.Empty;
                    txtClave.Text = dgvData.Rows[e.RowIndex].Cells[6].Value?.ToString() ?? string.Empty;

                    int estado;
                    if (int.TryParse(dgvData.Rows[e.RowIndex].Cells[7].Value?.ToString(), out estado))
                    {
                        cmbEstado.SelectedIndex = estado == 1 ? 0 : 1; 
                    }
                    else
                    {
                        MessageBox.Show("El valor del estado no es válido.");
                    }

                    string rol = dgvData.Rows[e.RowIndex].Cells[8].Value?.ToString() ?? string.Empty;
                    if (cmbRol.Items.Contains(rol))
                    {
                        cmbRol.SelectedItem = rol;
                    }
                    else
                    {
                        MessageBox.Show("El valor del rol no es válido.");
                    }

                    btnActualizar.Visible = true;
                    btnCancelar.Visible = true;
                    btnInsertar.Visible = false;
                    btnEliminar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el usuario: " + ex.Message);
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                string respuesta = "";

                if (txtNombre.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtNombre, "Ingrese el nombre del usuario");
                    return;
                }

                if (txtApellido.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtApellido, "Ingrese el apellido del usuario");
                    return;
                }

                if (txtUsuario.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtUsuario, "Ingrese el nombre de usuario");
                    return;
                }

                if (txtClave.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtClave, "Ingrese la contraseña");
                    return;
                }

                if (txtCorreo.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtCorreo, "Ingrese el correo electrónico");
                    return;
                }

                if (cmbEstado.SelectedIndex == -1)
                {
                    this.MensajeError("Falta seleccionar el estado del usuario");
                    error.SetError(cmbEstado, "Seleccione el estado del usuario");
                    return;
                }

                if (cmbRol.SelectedIndex == -1)
                {
                    this.MensajeError("Falta seleccionar el rol del usuario");
                    error.SetError(cmbRol, "Seleccione el rol del usuario");
                    return;
                }

                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string nombreUsuario = txtUsuario.Text;
                string contraseña = txtClave.Text;
                string email = txtCorreo.Text;
                int estado = cmbEstado.Text == "Activo" ? 1 : 0;  
                string rol = cmbRol.Text;

                respuesta = CN_Usuario.Insertar(nombre, apellido, nombreUsuario, contraseña, email, estado, rol);

                if (respuesta == "OK")
                {
                    this.MensajeOK("Registro completado correctamente.");
                    limpiar();
                    listar();
                }
                else
                {
                    this.MensajeError(respuesta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            btnInsertar.Visible = true;
            btnActualizar.Visible = false;
            btnCancelar.Visible = false;
        }

        private void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSeleccionar.Checked)
            {
                dgvData.Columns[0].Visible = true;
                btnEliminar.Visible = true;
            }
            else
            {
                dgvData.Columns[0].Visible = false;
                btnEliminar.Visible = false;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvData.Columns["Seleccionar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar = (DataGridViewCheckBoxCell)dgvData.Rows[e.RowIndex].Cells["Seleccionar"];
                chkEliminar.Value = !Convert.ToBoolean(chkEliminar.Value);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;
                string respuesta = "";
                int codigo;
                bool registroEliminado = false;

                Opcion = MessageBox.Show("¿Realmente desea eliminar al usuario?", "Control de Usuarios", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Opcion == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value)) 
                        {
                            codigo = Convert.ToInt32(row.Cells[1].Value);
                            respuesta = CN_Usuario.Eliminar(codigo); 

                            if (respuesta.Equals("OK"))
                            {
                                this.MensajeOK("Se eliminó el registro: " + Convert.ToString(row.Cells[2].Value) + "  " + Convert.ToString(row.Cells[3].Value));
                                registroEliminado = true;
                            }
                            else
                            {
                                this.MensajeError("Error al eliminar el registro: " + respuesta);
                            }
                        }
                    }

                    if (!registroEliminado)
                    {
                        this.MensajeError("No se seleccionó ningún registro para eliminar.");
                    }

                    this.listar(); 

                    btnEliminar.Visible = false;
                    chkSeleccionar.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Regresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
