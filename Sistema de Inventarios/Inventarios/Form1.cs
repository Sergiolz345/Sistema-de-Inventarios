using Conexiones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventarios
{
    public partial class Form1 : Form
    {
        public dbSqlServer conexion = null;

        public String sData = "";
        string sServer, sUsuario, sPasword;


        public Form1(string Server, string Usuario, string Pasword)
        {
            InitializeComponent();

            sServer = Server;
            sUsuario = Usuario;
            sPasword = Pasword;
            lblUser.Text = "Usuario: " + Usuario;

            conexion = new dbSqlServer(sServer, sUsuario, sPasword);
        }


        #region Mover Arrastrar Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void lblMenu_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pbUser_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void lblUser_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion


        #region Botones de Ventana
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion



        private void btnCambiar_Click(object sender, EventArgs e)
        {
            conexion.CerrarConexion();

            sServer = "";
            sUsuario = "";
            sPasword = "";
            lblUser.Text = "";

            this.Hide();

            Login c = new Login();
            c.ShowDialog();
        }


        private void btnProductos_Click(object sender, EventArgs e)
        {

            this.Hide();

            Productos c = new Productos(sServer, sUsuario, sPasword);
            c.ShowDialog();
        }


        private void btnInventario_Click(object sender, EventArgs e)
        {

            this.Hide();

            Inventarios c = new Inventarios(sServer, sUsuario, sPasword);
            c.ShowDialog();
        }
      
    }
}
