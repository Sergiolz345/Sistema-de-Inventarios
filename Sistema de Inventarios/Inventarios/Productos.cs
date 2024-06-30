using Conexiones;
using ReglasNegocio;
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
    public partial class Productos : Form
    {
        public OpProductos conexion = null;

        public String sData = "";
        string sServer, sUsuario, sPasword;


        public Productos(string Server, string Usuario, string Pasword)
        {
            InitializeComponent();

            sServer = Server;
            sUsuario = Usuario;
            sPasword = Pasword;


            conexion = new OpProductos (sServer, sUsuario, sPasword);
        }



        #region Mover Arrastrar Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void lblProductos_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Productos_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
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



        private void btnGuardar_Click(object sender, EventArgs e)
        {
            DatosProductos Datos = new DatosProductos();

            Datos.sProducto = tbProducto.Text;
            Datos.dPCompra = Convert.ToDouble(tbPCompra.Text);
            Datos.dPVenta = Convert.ToDouble(tbPVenta.Text);
            Datos.sDescripcion = tbDescripcion.Text;

         
            if (!conexion.Alta(Datos))
            {
                MessageBox.Show("Ocurrio un error: " + conexion.sLastError);
            }

            else
            {
                MessageBox.Show("Producto Registrado Exitosamente!....");
            }
        }


        private void tbProducto_Validating(object sender, CancelEventArgs e)
        {
            DataTable Table = new DataTable();
            Table = conexion.Llenado(tbProducto.Text);

            foreach (DataRow r in Table.Rows)
            {
                if (tbProducto.Text == r["ProductoID"].ToString())
                {
                    tbDescripcion.Text = (r["Descripcion"].ToString());
                    tbPCompra.Text = (r["PrecioCompra"].ToString());
                    tbPVenta.Text = (r["PrecioVenta"].ToString());
                }
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbProducto.Text = "";
            tbDescripcion.Text = "";
            tbPCompra.Text = "";
            tbPVenta.Text = "";
        }


        private void Productos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                BuscarProducto c = new BuscarProducto(sServer, sUsuario, sPasword);
                c.ShowDialog();

                if (c.g != null)
                {
                    tbProducto.Text = c.g;

                    DataTable Table = new DataTable();
                    Table = conexion.Llenado(tbProducto.Text);

                    foreach (DataRow r in Table.Rows)
                    {
                        if (tbProducto.Text == r["ProductoID"].ToString())
                        {
                            tbDescripcion.Text = (r["Descripcion"].ToString());
                            tbPCompra.Text = (r["PrecioCompra"].ToString());
                            tbPVenta.Text = (r["PrecioVenta"].ToString());
                        }
                    }
                }
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!conexion.Eliminar(tbProducto.Text))
            {
                MessageBox.Show("Ocurrio un error: " + conexion.sLastError);
            }

            else
            {
                MessageBox.Show("Producto Eliminado Exitosamente!....");
            }
        }


        private void btnVolverMenu_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form1 c = new Form1(sServer, sUsuario, sPasword);
            c.ShowDialog();
        }
    }
}
