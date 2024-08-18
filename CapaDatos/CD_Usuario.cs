using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public bool IniciarSesion(string nombre, string clave, out string mensaje)
        {
            mensaje = string.Empty;
            bool inicioCorrecto = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("iniciosesion", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Contraseña", clave);

                    // Parámetro de salida
                    SqlParameter outputParameter = new SqlParameter();
                    outputParameter.ParameterName = "@mensaje";
                    outputParameter.SqlDbType = SqlDbType.VarChar;
                    outputParameter.Size = 30;
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    // Recuperar el mensaje de salida
                    mensaje = cmd.Parameters["@mensaje"].Value.ToString();

                    if (mensaje.StartsWith("Bienvenido"))
                    {
                        inicioCorrecto = true;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error al intentar iniciar sesión: " + ex.Message;
            }

            return inicioCorrecto;
        }

        // Otros métodos para operaciones de usuario si es necesario
    }
}

