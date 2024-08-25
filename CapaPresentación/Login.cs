using CapaNegocios;
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
    public partial class Login : Form
    {
        private static Login instancia;

        public static Login Instancia
        {
            get
            {
                if (instancia == null || instancia.IsDisposed)
                    instancia = new Login();
                return instancia;
            }
        }

        public Login()
        {
            InitializeComponent();
            LimpiarCampos();

            txtuser.Text = "Usuario";
            txtuser.ForeColor = Color.DimGray;

            txtpass.Text = "Contraseña";
            txtpass.ForeColor = Color.DimGray;
            txtpass.UseSystemPasswordChar = false;
        }

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "Usuario")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.Black;
            }

        }
        public void LimpiarCampos()
        {
            txtuser.Text = "Usuario";
            txtuser.ForeColor = Color.DimGray;

            txtpass.Text = "Contraseña";
            txtpass.ForeColor = Color.DimGray;
            txtpass.UseSystemPasswordChar = false; 
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "Usuario";
                txtuser.ForeColor = Color.DimGray;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "Contraseña")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.Black;
                txtpass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                txtpass.Text = "Contraseña";
                txtpass.ForeColor = Color.DimGray;
                txtpass.UseSystemPasswordChar = false;
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string username = txtuser.Text.Trim();
            string password = txtpass.Text.Trim();
            string mensaje = string.Empty;

            CN_Usuario negocio = new CN_Usuario();

            try
            {
                bool autenticado = negocio.IniciarSesion(username, password, out mensaje);

                if (autenticado)
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    Inicio form = new Inicio(username);
                    form.Show();
                    this.Hide();

                    form.FormClosing += frm_closing;
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar iniciar sesión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            Login loginForm = Login.Instancia;
            loginForm.LimpiarCampos();
            loginForm.Show();
        }

    }
}
