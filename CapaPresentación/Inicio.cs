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
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White; 
            }

            menu.BackColor = Color.Silver;
            MenuActivo = menu; 

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.SteelBlue;

            PanelContenedor.Controls.Add(formulario); 
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

            this.Close();

            
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Form form = Application.OpenForms[i];
                if (form.Name != "Login")
                {
                    form.Close();
                }
            }

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

            matriculaForm.TopLevel = false;
            matriculaForm.FormBorderStyle = FormBorderStyle.None;
            matriculaForm   .Dock = DockStyle.Fill;

            PanelContenedor.Controls.Add(matriculaForm);

            matriculaForm.BringToFront();

            matriculaForm.Show();
        }

        private void Cerrarsesion2_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Login loginForm = Login.Instancia;
                loginForm.LimpiarCampos();

 
                loginForm.Show();
                this.Close(); 
            }
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            Usuario usuarioForm = new Usuario();

            usuarioForm.TopLevel = false;
            usuarioForm.FormBorderStyle = FormBorderStyle.None;
            usuarioForm.Dock = DockStyle.Fill;

            PanelContenedor.Controls.Add(usuarioForm);

            usuarioForm.BringToFront();

            usuarioForm.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Materia materiaForm = new Materia();

            materiaForm.TopLevel = false;
            materiaForm.FormBorderStyle = FormBorderStyle.None;
            materiaForm.Dock = DockStyle.Fill;

            PanelContenedor.Controls.Add(materiaForm);

            materiaForm.BringToFront();

            materiaForm.Show();
        }
    }
}
