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
    public partial class Inventarios : Form
    {
        public OpInventarios conexion = null;

        public String sData = "";
        string sServer, sUsuario, sPasword, Dia;
        int Row = 0, p = 0;
        Double FolioT;


        public Inventarios(string Server, string Usuario, string Pasword)
        {
            InitializeComponent();

            sServer = Server;
            sUsuario = Usuario;
            sPasword = Pasword;

            conexion = new OpInventarios(sServer, sUsuario, sPasword);
        }


        #region Mover Arrastrar Formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void lblInventarios_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Inventarios_MouseDown(object sender, MouseEventArgs e)
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



        private void Inventarios_Load(object sender, EventArgs e)
        {
            Dia = dtFecha.Text;

            DataTable data = new DataTable();
            OpInventarios D = new OpInventarios(sServer, sUsuario, sPasword);
            data = D.Llenado2();

            int c = data.Rows.Count;

            if (c == 0)
            {
                tbFolio.Text = "1";
                FolioT = 1;
            }

            else
            {
                foreach (DataRow r in data.Rows)
                {
                    FolioT = Convert.ToDouble(r["Folio"].ToString()) + 1;
                    tbFolio.Text = FolioT.ToString();
                }
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            DatosInventarios Datos = new DatosInventarios();

            //Datos.sTipoMovimiento = cbTMovimiento.Text;
            //Datos.dFecha = dtFecha.Value;
            //Datos.nFolio = Convert.ToInt32(tbFolio.Text);

            DataTable dt = new DataTable();
            dt.Columns.Add("Row_ID", typeof(int));
            dt.Columns.Add("ProductoID", typeof(string));
            dt.Columns.Add("Folio", typeof(int));
            dt.Columns.Add("Precio", typeof(double));
            dt.Columns.Add("Cantidad", typeof(double));
            dt.Columns.Add("Sucursal", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateTime));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("TipoMovimiento", typeof(string));

            Double dPrecio = Convert.ToDouble(tbPrecio.Text);
            Double dCantidad = Convert.ToDouble(tbCantidad.Text);
            int Row_ID;

            if (p == 0)
            {
                Row_ID = 1;
                Double dTotal = dPrecio * dCantidad;

                dt.Rows.Add(Row_ID, tbProducto.Text, tbFolio.Text, Convert.ToDouble(tbPrecio.Text),
                    Convert.ToDouble(tbCantidad.Text), tbSucursal.Text, dtFecha.Value,
                    dTotal, cbTMovimiento.Text);
            }

            else
            {

                int y;
                y = dataGridView1.RowCount;
                Row_ID = 1;
                for (int i = 0; i < y; i++)
                {
                    dt.Rows.Add(Row_ID, Convert.ToString(dataGridView1.Rows[i].Cells[0].Value),
                    tbFolio.Text, Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value), 
                    Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value), tbSucursal.Text, 
                    dtFecha.Value, Convert.ToDouble(tbTotal.Text), cbTMovimiento.Text);

                    Row_ID++;
                }
            }

            Datos.Data(dt);


            if (!conexion.Alta(Datos))
            {
                MessageBox.Show("Ocurrio un error: " + conexion.sLastError);
            }

            else
            {
                FolioT = FolioT + 1;
                tbFolio.Text = FolioT.ToString();

                MessageBox.Show("Registro Guardado Exitosamente!....");
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbProducto.Text = "";
            tbDescripcion.Text = " Elige un Producto";
            tbDescripcion.Enabled = false;
            tbCantidad.Text = "";
            tbPrecio.Text = "";

            tbFolio.Text = "";
            tbSucursal.Text = "";
            cbTMovimiento.Text = "";
            dtFecha.Text = Dia;
            tbTotal.Text = "";
            p = 0;

            DataTable data = new DataTable();
            OpInventarios D = new OpInventarios(sServer, sUsuario, sPasword);
            data = D.Llenado2();
           
                foreach (DataRow r in data.Rows)
                {
                    FolioT = Convert.ToDouble(r["Folio"].ToString()) + 1;
                    tbFolio.Text = FolioT.ToString();
                }
 
            FolioT = FolioT;
            tbFolio.Text = FolioT.ToString();

            dataGridView1.Rows.Clear();
        }


        private void Inventarios_KeyDown(object sender, KeyEventArgs e)
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

                            if (cbTMovimiento.Text == "Entrada")
                            {
                                tbPrecio.Text = (r["PrecioCompra"].ToString());
                            }

                            else
                            {
                                tbPrecio.Text = (r["PrecioVenta"].ToString());
                            }
                        }
                    }
                }
            }
        }


        private void tbProducto_Validating(object sender, CancelEventArgs e)
        {
            if (!conexion.ExisteProducto(tbProducto.Text))
            {
                MessageBox.Show("Producto no Existente!...");
                return;
            }

            else
            {
                DataTable Table = new DataTable();
                Table = conexion.Llenado(tbProducto.Text);
                tbDescripcion.Enabled = true;

                foreach (DataRow r in Table.Rows)
                {
                    if (tbProducto.Text == r["ProductoID"].ToString())
                    {
                        tbDescripcion.Text = (r["Descripcion"].ToString());

                        if(cbTMovimiento.Text == "Entrada")
                        {
                            tbPrecio.Text = (r["PrecioCompra"].ToString());
                        }

                        else
                        {
                            tbPrecio.Text = (r["PrecioVenta"].ToString());
                        }
                    }
                }
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!conexion.Eliminar(tbFolio.Text))
            {
                MessageBox.Show("Ocurrio un error: " + conexion.sLastError);
            }

            else
            {
                MessageBox.Show("Registro Eliminado Exitosamente!....");
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (tbProducto.Text == "" || tbPrecio.Text == "" || tbCantidad.Text == "")
            {
                MessageBox.Show("Termine de Ingresar Datos...");
            }

            else
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[Row].Cells[0].Value = tbProducto.Text;
                dataGridView1.Rows[Row].Cells[1].Value = tbDescripcion.Text;
                dataGridView1.Rows[Row].Cells[2].Value = tbCantidad.Text;
                dataGridView1.Rows[Row].Cells[3].Value = tbPrecio.Text;
                dataGridView1.Rows[Row].Cells[4].Value = " $ " + Convert.ToDouble(tbPrecio.Text) * Convert.ToDouble(tbCantidad.Text);
                Double n = Convert.ToDouble(tbPrecio.Text) * Convert.ToDouble(tbCantidad.Text);
                Double t = Convert.ToDouble(tbTotal.Text) + n;
                tbTotal.Text = Convert.ToString(t);
                Row = Row + 1;
                p = 1;
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
