using RayPro.Aplicaciones;
using RayPro.Aplicaciones.tools;
using RayPro.configuraciones;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace RayPro.Vista
{
    public partial class Login : Form
    {

        Settings configurar= Settings.Default;
        public Login()
        {
            InitializeComponent();
            loadCmboUser();
            // Habilitar la propiedad KeyPreview para capturar las pulsaciones de teclas en el formulario
            this.KeyPreview = true;

            // Suscribir el evento KeyDown para manejar las pulsaciones de teclas
            this.KeyDown += Login_KeyDown;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text.Equals("Contraseña"))
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.DimGray;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Contraseña";
                txtPassword.ForeColor = Color.DimGray;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        /*BOTON DE ACCEDER AL LOGIN*/
        private void btnAcceder_Click(object sender, EventArgs e)
        {
           
          InitializationLoginSystem();
        }

        private void linkSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrQuestion question = new FrQuestion();
            question.ShowDialog();
            Hide();
        }



        //=====================================METODOS AUXILIARES==========================================================//

        private void mensajeDeError(String msge)
        {
            Timer temporizador = new Timer();
            temporizador.Interval = 5000;

            lblErrorMsg.Text = "   " + msge;
            lblErrorMsg.ForeColor = Color.OrangeRed;
            lblErrorMsg.Visible = true;

            temporizador.Tick += (sender, e) =>
            {
                // Oculta el labelText cuando el temporizador alcance los 5 segundos
                lblErrorMsg.Visible = false;

                // Detiene el temporizador
                temporizador.Stop();
            };

            // Inicia el temporizador
            temporizador.Start();
        }

        private void limpiar()
        {
            cboUsuario.SelectedIndex = 0; // selecciona el primero
            txtPassword.Clear();
            txtPassword.Focus();
        }

        private void loadCmboUser()
        {
            cboUsuario.Items.Add(configurar.Usuarios);
            cboUsuario.Items.Add("Admin");
            cboUsuario.SelectedIndex = 0;

            cboUsuario.DrawMode = DrawMode.OwnerDrawFixed;
            cboUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            cboUsuario.DrawItem += cboUsuario_DrawItem;
        }
        

        private void InitializationLoginSystem()
        {
            string rol = cboUsuario.SelectedItem?.ToString();

            if (!LicenciaManager.ValidarAlIniciar())
            {
                MessageBox.Show("Producto no autorizado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAcceder.Enabled = false;
                return;
            }

            if (rol == configurar.Usuarios && txtPassword.Text == configurar.PassUser)
            {
                MainRayX frMain = new MainRayX();

                Welcome frWelcome = new Welcome(frMain);
                frWelcome.ShowDialog();

                Hide();
            }
            else if (rol.Equals("Admin") && txtPassword.Text == "configuracion7070")
            {
                SettingDev frDev = new SettingDev();
                frDev.ShowDialog();
                Hide();
            }
            else
            {
                mensajeDeError("Ingrese Usuario o Contraseña, Campos Vacíos...");
                limpiar();
            }
        }
 //===================================================================================================================//
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Down)
            {
                txtPassword.Focus();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnAcceder.PerformClick();
            }
        }

        private void cboUsuario_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;

            var cb = (System.Windows.Forms.ComboBox)sender;
            string text = cb.GetItemText(cb.Items[e.Index]);

            using (var sf = new StringFormat())
            using (var brush = new SolidBrush(e.ForeColor))
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds, sf);
            }

            e.DrawFocusRectangle();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            cboUsuario.Invalidate();
        }


    }
}
    

