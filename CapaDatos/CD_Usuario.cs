using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using DocumentFormat.OpenXml.Spreadsheet;

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
                using (SqlConnection connection = new Conexion().ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("iniciosesion", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Contraseña", clave);

                    SqlParameter outputParameter = new SqlParameter
                    {
                        ParameterName = "@mensaje",
                        SqlDbType = SqlDbType.NVarChar, 
                        Size = 30,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParameter);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    
                    mensaje = cmd.Parameters["@mensaje"].Value.ToString();

                    
                    if (mensaje.StartsWith("Bienvenid@: "))
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

        public DataTable listar()
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand comando = new SqlCommand("listar_usuarios", conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    resultado = comando.ExecuteReader();
                    tabla.Load(resultado);
                    return tabla;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                }
            }
        }

        public DataTable Buscar(string valor)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("buscar_usuarios", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@valor", SqlDbType.NVarChar).Value = valor;
                    conexion.Open();
                    using (SqlDataReader resultado = command.ExecuteReader())
                    {
                        tabla.Load(resultado);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }
            return tabla;
        }
        public string Insertar(Usuario obj)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("insertar_usuario", conexion);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100)).Value = obj.Nombre;
                    command.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.NVarChar, 100)).Value = obj.Apellido;
                    command.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50)).Value = obj.NombreUsuario;
                    command.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.NVarChar, 255)).Value = obj.Contraseña;
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = obj.Email;
                    command.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Int)).Value = obj.Estado;
                    command.Parameters.Add(new SqlParameter("@Rol", SqlDbType.NVarChar, 50)).Value = obj.Rol;

                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo ingresar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }
            return respuesta;
        }
        public string Actualizar(Usuario obj)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("actualizar_usuario", conexion); 
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)).Value = obj.IdUsuario;
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100)).Value = obj.Nombre;
                    command.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.NVarChar, 100)).Value = obj.Apellido;
                    command.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.NVarChar, 50)).Value = obj.NombreUsuario;
                    command.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.NVarChar, 255)).Value = obj.Contraseña;
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100)).Value = obj.Email;
                    command.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Int)).Value = obj.Estado;
                    command.Parameters.Add(new SqlParameter("@Rol", SqlDbType.NVarChar, 50)).Value = obj.Rol;

                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo actualizar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }
            return respuesta;
        }
        public string Eliminar(int IdUsuario)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("eliminar_usuario", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)).Value = IdUsuario;

                    conexion.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    respuesta = filasAfectadas > 0 ? "OK" : "No se pudo eliminar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open)
                        conexion.Close();
                }
            }
            return respuesta;
        }


    }
}

