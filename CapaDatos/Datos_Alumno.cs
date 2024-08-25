using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;

namespace CapaDatos
{
    public class Datos_Alumno
    {
        public DataTable listar()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("listar_estudiantes", conexion);
                    command.CommandType = CommandType.StoredProcedure;
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



        public DataTable Buscar(string valor)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = new Conexion().ObtenerConexion())

            {
                try
                {
                    SqlCommand command = new SqlCommand("buscar_estudiantes", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Valor", SqlDbType.VarChar).Value = valor;
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
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
            return tabla;
        }

        public string Insertar(string nombre, string apellido, DateTime fechaNacimiento, string telefono, int estado, int idMateria)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("insertar_estudiante", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar)).Value = nombre;
                    command.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.NVarChar)).Value = apellido;
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", SqlDbType.Date)).Value = fechaNacimiento;
                    command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.NVarChar)).Value = telefono;
                    command.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Int)).Value = estado;
                    command.Parameters.Add(new SqlParameter("@IdMateria", SqlDbType.Int)).Value = idMateria;

                    conexion.Open();
                    
                    
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




        public string Actualizar(Alumno obj)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("Actualizar_estudiante", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdEstudiantes", SqlDbType.Int)).Value = obj.IdEstudiante;
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar)).Value = obj.Nombre;
                    command.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar)).Value = obj.Apellido;
                    command.Parameters.Add(new SqlParameter("@FechaNacimiento", SqlDbType.Date)).Value = obj.FechaNacimiento;
                    command.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar)).Value = obj.Telefono;
                    command.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Int)).Value = obj.Estado;
                    command.Parameters.Add(new SqlParameter("@IdMateria", SqlDbType.Int)).Value = obj.IdMateria;

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

        public string Eliminar(int id)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("Eliminar_estudiantes", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo eliminar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
            return respuesta;
        }

        public string Activar(int id)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())


            {
                try
                {
                    SqlCommand command = new SqlCommand("Activar_estudiantes", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo activar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
            return respuesta;
        }

        public string Desactivar(int id)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())

            {
                try
                {
                    SqlCommand command = new SqlCommand("Desactivar_estudiantes", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo desactivar el registro";
                }
                catch (Exception ex)
                {
                    respuesta = ex.Message;
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
            return respuesta;
        }
    }
}

