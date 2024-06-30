using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class dbSqlServer
    {
        public String sDatabase;
        public string sLastError = "";
        public SqlConnection conexion;

        public dbSqlServer(string sServer, string sUsuario, string sPasword)
        {
            conexion = new SqlConnection($"Server={sServer};Database=master;User Id={sUsuario};Password={sPasword};");
        }

        public Boolean AbrirConexion()
        {
            Boolean bALLOK = true;

            try
            {
                conexion.Open();
            }
            catch (Exception EX)
            {
                sLastError = EX.Message;
                bALLOK = false;
            }
            return bALLOK;
        }


        public Boolean ConexionAbierta()
        {
            Boolean bALLOK = true;
            try
            {
                bALLOK = conexion.State == System.Data.ConnectionState.Open ? true : false;
            }
            catch (Exception EX)
            {
                sLastError = EX.Message;
                bALLOK = false;
            }
            return bALLOK;
        }


        public Boolean EjecutarCommando(String sCmd)
        {
            Boolean bALLOK = true;

            conexion.Open();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sCmd, conexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception EX)
            {
                bALLOK = false;
                sLastError = EX.Message;
            }
            conexion.Close();

            return bALLOK;
        }


        public void CerrarConexion()
        {
            conexion.Close();
        }
    }
}
