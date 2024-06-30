using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReglasNegocio
{
    public class DatosProductos
    {
        public String sProducto;
        public Double dPCompra;
        public Double dPVenta;
        public String sDescripcion;
    }


    public class OpProductos
    {
        public SqlConnection conn;

        public String sLastError = "";

        string sServer, sUsuario, sPasword;

        public OpProductos(string Server, string Usuario, string Pasword)
        {
            sServer = Server;
            sUsuario = Usuario;
            sPasword = Pasword;
        }


        public Boolean Alta(DatosProductos datos)
        {
            Boolean bAllok = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductoID", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("PrecioCompra", typeof(double));
            dt.Columns.Add("PrecioVenta", typeof(double));

            dt.Rows.Add(datos.sProducto, datos.sDescripcion, datos.dPCompra, datos.dPVenta);

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {

                conn.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand("Agregar_Producto", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Product_List", SqlDbType.Structured).Value = dt;
                    cmd.ExecuteNonQuery();


                    conn.Close();
                }

                catch (Exception ex)
                {
                    bAllok = false;
                    sLastError = ex.Message;

                    conn.Close();
                }
            }

            return bAllok;
        }


        public Boolean Eliminar(String sProductID)
        {
            Boolean bAllok = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductoID", typeof(string));

            dt.Rows.Add(sProductID);

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("Borrar_Producto", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Delete_List", SqlDbType.Structured).Value = dt;
                    cmd.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    bAllok = false;

                    sLastError = sLastError + ex;
                }

                conn.Close();
            }

            return bAllok;
        }


        public DataTable Buscar(String Dato)
        {
            DataTable Table = new DataTable();
            string dt = $"'%{Dato}%'";

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"Buscar_Product {dt}", conn);

                Table.Load(cmd.ExecuteReader());

                conn.Close();
            }

            return Table;
        }


        public DataTable Llenado(String sProductID)
        {
            DataTable Table = new DataTable();
            string dt = $"'{sProductID}'";

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"sp_Select_Product {dt}", conn);

                Table.Load(cmd.ExecuteReader());

                conn.Close();
            }

            return Table;
        }



    }
}
