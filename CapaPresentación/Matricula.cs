using CapaNegocios;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace CapaPresentación
{
    public partial class Matricula : Form
    {
        public Matricula()
        {
            InitializeComponent();
        }
        private void limpiar()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();

            btnRegistrar.Visible = true;
            error.Clear();
        }
        private void listar()
        {
            try
            {
                dgvData.DataSource = CN_Estudiante.listar();
                this.formato();
                lblTotal.Text = "Total registros: " + Convert.ToString(dgvData.Rows.Count);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void formato()
        {
            dgvData.Columns[0].Visible = false; // seleccionar
            dgvData.Columns[1].Visible = false; // id
            dgvData.Columns[2].Width = 80;
            dgvData.Columns[3].Width = 80;
            dgvData.Columns[4].Width = 100;
            dgvData.Columns[5].Width = 125;

            if (dgvData.Columns.Contains("EstadoNumerico"))
            {
                dgvData.Columns["EstadoNumerico"].Visible = false;
            }

            
            if (dgvData.Columns.Contains("EstadoTexto"))
            {
                dgvData.Columns["EstadoTexto"].HeaderText = "Estado";
                dgvData.Columns["EstadoTexto"].Width = 80; 
            }

            dgvData.CellFormatting -= dgvData_CellFormatting;

            
            dgvData.CellFormatting += dgvData_CellFormatting;
        }

        private void dgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
            if (dgvData.Columns[e.ColumnIndex].Name == "Estado")
            {
                if (e.Value != null)
                {
                    
                    e.Value = e.Value.ToString() == "1" ? "Activo" : "Inactivo";
                    e.FormattingApplied = true;
                }
            }
        }

        private void Regresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Matricula_Resize(object sender, EventArgs e)
        {
            labelTitulo.Left = (this.Width - labelTitulo.Width) / 2;
        }

        private void Regresar_MouseHover(object sender, EventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(Regresar, "Regresar al menú principal");
        }


        private void Matricula_Load(object sender, EventArgs e)
        {
            this.listar();

            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");


            CargarMaterias();

        }
        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de alumnos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void MensajeOK(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Control de alumnos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


       
        private void BuscarEstudiante()
        {

            try
            {
                string valorBusqueda = txtBuscar.Text;
                dgvData.DataSource = CN_Estudiante.Buscar(valorBusqueda);
                this.formato();
                lblTotal.Text = "Total registros: " + Convert.ToString(dgvData.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void Buscar_Click(object sender, EventArgs e)
        {
            this.BuscarEstudiante();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    this.MensajeError("Falta ingresar el nombre del estudiante.");
                    error.SetError(txtNombre, "Ingrese el nombre del estudiante.");
                    return;
                }

                if (string.IsNullOrEmpty(txtApellido.Text))
                {
                    this.MensajeError("Falta ingresar el apellido del estudiante.");
                    error.SetError(txtApellido, "Ingrese el apellido del estudiante.");
                    return;
                }

                if (string.IsNullOrEmpty(txtTelefono.Text))
                {
                    this.MensajeError("Falta ingresar el teléfono.");
                    error.SetError(txtTelefono, "Ingrese el teléfono.");
                    return;
                }

                if (string.IsNullOrEmpty(txtFecha.Text) || !DateTime.TryParse(txtFecha.Text, out DateTime fechaNacimiento))
                {
                    this.MensajeError("La fecha de nacimiento no es válida.");
                    error.SetError(txtFecha, "Ingrese una fecha válida.");
                    return;
                }

                if (cmbMateria.SelectedIndex == -1)
                {
                    this.MensajeError("Debe seleccionar una materia.");
                    error.SetError(cmbMateria, "Seleccione una materia.");
                    return;
                }

                if (cmbEstado.SelectedIndex == -1)
                {
                    this.MensajeError("Debe seleccionar el estado del estudiante.");
                    error.SetError(cmbEstado, "Seleccione el estado del estudiante.");
                    return;
                }

                // Obtener valores
                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string telefono = txtTelefono.Text;
                int estado = cmbEstado.Text == "Activo" ? 1 : 0;
                int idMateria = (int)cmbMateria.SelectedValue;

                // Registrar estudiante
                string respuesta = CN_Estudiante.Insertar(nombre, apellido, fechaNacimiento, telefono, estado, idMateria);

                // Mostrar resultado
                if (respuesta.StartsWith("Estudiante registrado exitosamente"))
                {
                    this.MensajeOK(respuesta);
                    LimpiarFormulario();
                    this.listar(); // Actualizar DataGridView
                }
                else
                {
                    this.MensajeError("Error al registrar el estudiante: " + respuesta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el estudiante: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void CargarMaterias()
        {
            try
            {
                DataTable dtMaterias = CN_Materia.Listar();

                // Imprimir los nombres de las columnas para depuración
                foreach (DataColumn column in dtMaterias.Columns)
                {
                    Console.WriteLine("Columna: " + column.ColumnName);
                }

                // Asegúrate de que el DataTable contiene las columnas necesarias
                if (dtMaterias.Columns.Contains("Nombre") && dtMaterias.Columns.Contains("IdMaterias"))
                {
                    cmbMateria.DataSource = dtMaterias;
                    cmbMateria.DisplayMember = "Nombre";
                    cmbMateria.ValueMember = "IdMaterias"; // Cambia a IdMaterias
                }
                else
                {
                    MessageBox.Show("El DataTable no contiene las columnas esperadas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las materias: " + ex.Message);
            }
        }
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtFecha.Clear();
            cmbMateria.SelectedIndex = -1;
            cmbEstado.SelectedIndex = -1;
        }
    }
}
