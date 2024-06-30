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
    public partial class BuscarProducto : Form
    {
        public OpProductos conexion = null;

        public String sData = "";
        string sServer, sUsuario, sPasword;
        public string g;

        public BuscarProducto(string Server, string Usuario, string Pasword)
        {
            InitializeComponent();

            sServer = Server;
            sUsuario = Usuario;
            sPasword = Pasword;

            conexion = new OpProductos(sServer, sUsuario, sPasword);
        }



        #region Mover Arrastrar Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void lblBuscar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BuscarProducto_MouseDown(object sender, MouseEventArgs e)
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
           this.Close();
        }


        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion



        private void btnBuscar_Click(object sender, EventArgs e)
        { 
            DataTable dt = new DataTable();
            int Row = 0;

            dt = conexion.Buscar(tbDato.Text);
            int n = dt.Rows.Count;

            if(n == 0)
            {
                MessageBox.Show("Producto no encontrado!...");
            }

            else
            {
                dataGridView1.Rows.Clear();

                foreach (DataRow r in dt.Rows)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[Row].Cells[0].Value = r["ProductoID"].ToString();
                    dataGridView1.Rows[Row].Cells[1].Value = r["Descripcion"].ToString();
                    dataGridView1.Rows[Row].Cells[2].Value = r["PrecioCompra"].ToString();
                    dataGridView1.Rows[Row].Cells[3].Value = r["PrecioVenta"].ToString();
                    Row = Row + 1;
                }
            }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string date = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
            {
                this.Hide();

                Productos a = new Productos(sServer, sUsuario, sPasword);
                g = date;
            }
        }
    }
}
