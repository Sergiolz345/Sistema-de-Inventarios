using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReglasNegocio
{
    public class DatosInventarios
    {
        public DataTable dt = new DataTable();

        public DataTable Data(DataTable dt1)
        {
            dt = dt1;
            return dt;
        }
    }

    public class OpInventarios
    {
        SqlCommand cmd;

        public SqlConnection conn;

        public String sLastError = "";

        string sServer, sUsuario, sPasword;

        public OpInventarios(string Server, string Usuario, string Pasword)
         {
            sServer = Server;
            sUsuario = Usuario; 
            sPasword = Pasword;
         }

        public Boolean Alta(DatosInventarios datos)
        {
            Boolean bAllok = true;
            DataTable dt = new DataTable();
            dt = datos.dt;

            using (SqlConnection Conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                Conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("Agregar_Inventarios", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Data", SqlDbType.Structured).Value = dt;
                    cmd.ExecuteNonQuery();

                    Conn.Close();
                }

                catch (Exception ex)
                {
                    bAllok = false;
                    sLastError = ex.Message;

                    Conn.Close();
                }

                return bAllok;
            }
        }


        public Boolean Eliminar(String sFolio)
        {
            Boolean bAllok = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("Row_ID", typeof(int));
            dt.Columns.Add("ProductoID", typeof(string));
            dt.Columns.Add("Folio", typeof(int));
            dt.Columns.Add("Cantidad", typeof(double));
            dt.Columns.Add("Sucursal", typeof(string));
            int Row_ID;


            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                DataTable Table = new DataTable();

                conn.Open();


                SqlCommand strCmd = new SqlCommand();
                strCmd.Connection = conn;


                strCmd.CommandText = "SELECT * FROM InventarioDetalle " +
                           $"WHERE Folio = '{sFolio}';";

                Table.Load(strCmd.ExecuteReader());

                Row_ID = 1;

                foreach (DataRow r in Table.Rows)
                {
                    dt.Rows.Add(Row_ID, r["ProductoID"].ToString(),
                    r["Folio"].ToString(), r["Cantidad"].ToString(), r["Sucursal"].ToString());

                    Row_ID++;
                }

                try
                {
                    SqlCommand cmd = new SqlCommand("Borrar_Inventarios", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Delete_List", SqlDbType.Structured).Value = dt;
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


        public Boolean ExisteProducto(String sProductID)
        {
            Boolean bExist = true;

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                conn.Open();

                String strCmd = "SELECT COUNT (*) " +
                               $"FROM Productos " +
                               $"WHERE ProductoID = '{sProductID}';";

                SqlCommand cmd = new SqlCommand(strCmd, conn);

                if (Int32.Parse(cmd.ExecuteScalar().ToString()) <= 0)
                {
                    bExist = false;
                }

                conn.Close();
            }

            return bExist;
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


        public DataTable Llenado2()
        {
            DataTable Table = new DataTable();

            using (SqlConnection conn = new SqlConnection($"Server={sServer};Database=Inventarios;User Id={sUsuario};Password={sPasword};"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"sp_Select_Inventario", conn);

                Table.Load(cmd.ExecuteReader());

                conn.Close();
            }

            return Table;
        }

    }
}
