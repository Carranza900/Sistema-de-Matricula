using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ClosedXML.Excel.XLPredefinedFormat;
using System.Runtime.InteropServices;
using Irony.Parsing;

namespace CapaPresentación
{
    public partial class Inicio : Form
    {
        private static Form FormularioActivo = null;
        private Control MenuActivo = null;

        private string nombreUsuario;
        public Inicio(string nombre)
        {
            InitializeComponent();
            this.nombreUsuario = nombre;


        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hund, int wmsg, int wparam, int Iparam);
        private void AbrirFormulario(Control menu, Form formulario)
        {
            // Restaurar color de fondo de menú activo anterior
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White; // Suponiendo que MenuActivo es una variable miembro
            }

            // Cambiar color de fondo del menú seleccionado
            menu.BackColor = Color.Silver;
            MenuActivo = menu; // Asignar el menú activo actual

            // Cerrar formulario activo anterior
            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            // Mostrar nuevo formulario
            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.SteelBlue;

            PanelContenedor.Controls.Add(formulario); // 'contenedor' es el control donde se mostrará el formulario
            formulario.Show();


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 62)
            {
                MenuVertical.Width = 167;
            }
            else
            {
                MenuVertical.Width = 62;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // Cerrar el formulario actual (Inicio)
            this.Close();

            // Cerrar otros formularios abiertos, si los hay
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name != "Login") // O cualquier otro formulario que no deba cerrarse
                {
                    form.Close(); this.WindowState = FormWindowState.Normal;
                    btnRestaurar.Visible = false;
                    btnMaximizar.Visible = true;
                }
            }

            // Finalmente, cerrar la aplicación
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnRestaurar.Visible = true;
            btnMaximizar.Visible = false;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Matricular_Click(object sender, EventArgs e)
        {
            Matricula matriculaForm = new Matricula();

            // Establecer propiedades del formulario hijo
            matriculaForm.TopLevel = false;
            matriculaForm.FormBorderStyle = FormBorderStyle.None;
            matriculaForm   .Dock = DockStyle.Fill;

            // Agregar el formulario hijo al PanelContenedor del formulario Inicio
            PanelContenedor.Controls.Add(matriculaForm);

            // Asegurarse de que el formulario hijo esté en la parte superior
            matriculaForm.BringToFront();

            // Mostrar el formulario hijo
            matriculaForm.Show();
        }

        private void Cerrarsesion2_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar la respuesta del usuario
            if (resultado == DialogResult.Yes)
            {
                // Limpiar campos usando el método público en Login
                Login loginForm = Login.Instancia;
                loginForm.LimpiarCampos();

                // Mostrar el formulario de Login y cerrar el formulario actual (Inicio)
                loginForm.Show();
                this.Close(); // Cierra el formulario actual (Inicio)
            }
        }
    }
}
