using CapaNegocios;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentación
{
    public partial class Materia : Form
    {
        public Materia()
        {
            InitializeComponent();
        }

        private void listar()
        {
            try
            {
                dgvData1.DataSource = CN_Materia.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void Materia_Load(object sender, EventArgs e)
        {
            this.listar();
            this.formato();

            lblTotal.Text = "Total registros: " + dgvData1.Rows.Count;

            txtId.Visible = false;
            btnAgregar.Visible = true; 
            btnEliminar.Visible = true; 
            btnActualizar.Visible = false;
            btnCancelar.Visible = false;
        }

        private void formato()
        {
            dgvData1.Columns[0].Visible = false; // seleccionar
            dgvData1.Columns[1].Visible = false; // id
            dgvData1.Columns[2].Width = 100;
            dgvData1.Columns[3].Width = 120;
            dgvData1.Columns[4].Width = 80;
            dgvData1.Columns[5].Visible = false;
        }

        private void BuscarMateria()
        {

            try
            {
                string valorBusqueda = txtbuscar.Text;
                dgvData1.DataSource = CN_Materia.Buscar(valorBusqueda);
                this.formato();
                lblTotal.Text = "Total registros: " + Convert.ToString(dgvData1.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            this.BuscarMateria();
        }

        private void MensajeOK(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de alumnos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void limpiar()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtCreditos.Clear();
            btnActualizar.Visible = false;
            btnAgregar.Visible = true;
            error.Clear();

            dgvData1.Columns[0].Visible = false;
            btnEliminar.Visible = false;
            chkSeleccionar.Checked = false;
        }

        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de alumnos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string respuesta = "";

                if (txtNombre.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtNombre, "Ingrese el nombre de la materia");
                    return;
                }

                if (txtDescripcion.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtDescripcion, "Ingrese la descripción de la materia");
                    return;
                }

                if (txtCreditos.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtCreditos, "Ingrese los créditos disponibles");
                    return;
                }

                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int creditos;

                // Intentar convertir el texto a un entero
                if (!int.TryParse(txtCreditos.Text, out creditos) || creditos < 0)
                {
                    this.MensajeError("El valor de créditos no es válido. Ingrese un número entero positivo.");
                    error.SetError(txtCreditos, "Ingrese un número entero positivo válido para los créditos.");
                    return;
                }

                respuesta = CN_Materia.Insertar(nombre, descripcion, creditos);

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

        private void dgvData1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    this.limpiar();
                    btnActualizar.Visible = true;
                    btnCancelar.Visible = true;
                    btnAgregar.Visible = false;

                    txtId.Text = dgvData1.Rows[e.RowIndex].Cells[1].Value != null
                        ? dgvData1.Rows[e.RowIndex].Cells[1].Value.ToString()
                        : string.Empty;

                    txtNombre.Text = dgvData1.Rows[e.RowIndex].Cells[2].Value != null
                        ? dgvData1.Rows[e.RowIndex].Cells[2].Value.ToString()
                        : string.Empty;

                    txtDescripcion.Text = dgvData1.Rows[e.RowIndex].Cells[3].Value != null
                        ? dgvData1.Rows[e.RowIndex].Cells[3].Value.ToString()
                        : string.Empty;

                    txtCreditos.Text = dgvData1.Rows[e.RowIndex].Cells[4].Value != null
                        ? dgvData1.Rows[e.RowIndex].Cells[4].Value.ToString()
                        : string.Empty;
                    // TabG.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar la materia: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string respuesta = "";

                if (txtId.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtId, "Ingrese el Id de la materia");
                    return;
                }

                if (txtNombre.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtNombre, "Ingrese el nombre de la materia");
                    return;
                }

                if (txtDescripcion.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtDescripcion, "Ingrese la descripción de la materia");
                    return;
                }

                if (txtCreditos.Text == string.Empty)
                {
                    this.MensajeError("Falta ingresar algunos datos para completar el registro");
                    error.SetError(txtCreditos, "Ingrese los créditos disponibles");
                    return;
                }

                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                int creditos;

                if (!int.TryParse(txtCreditos.Text, out creditos) || creditos < 0)
                {
                    this.MensajeError("El valor de créditos no es válido. Ingrese un número entero positivo.");
                    error.SetError(txtCreditos, "Ingrese un número entero positivo válido para los créditos.");
                    return;
                }

                int idMateria = Convert.ToInt32(txtId.Text); 

                respuesta = CN_Materia.Actualizar(idMateria, nombre, descripcion, creditos);

                if (respuesta == "OK")
                {
                    this.MensajeOK("Registro actualizado correctamente.");
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

        private void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSeleccionar.Checked)
            {
                dgvData1.Columns[0].Visible = true;
                btnEliminar.Visible = true;
            }
                else
                {
                dgvData1.Columns[0].Visible = false;
                btnEliminar.Visible = false;
                }
        }

        private void dgvData1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgvData1.Columns["Seleccionar"].Index)
            {
                DataGridViewCheckBoxCell chkEliminar = (DataGridViewCheckBoxCell)dgvData1.Rows[e.RowIndex].Cells["Seleccionar"];
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

                Opcion = MessageBox.Show("¿Realmente desea eliminar el curso?", "Control de Materias", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Opcion == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dgvData1.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value)) 
                        {
                            codigo = Convert.ToInt32(row.Cells[1].Value);
                            respuesta = CN_Materia.Eliminar(codigo);

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;
            btnCancelar.Visible = false;
        }

        private void Materia_Resize(object sender, EventArgs e)
        {
            labelTitulo.Left = (this.Width - labelTitulo.Width) / 2;
        }

        private void Regresar_MouseHover(object sender, EventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Regresar, "Regresar al menú principal");
        }

        private void Regresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
