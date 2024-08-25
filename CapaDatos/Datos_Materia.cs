using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos
{
    public class Datos_Materia
    {
        public DataTable Listar()
        {
            DataTable dt = new DataTable();
            Conexion conexion = new Conexion(); 
            using (SqlConnection con = conexion.ObtenerConexion()) 
            {
                using (SqlCommand cmd = new SqlCommand("listar_materias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al listar las materias: " + ex.Message);
                        }
                    }
                }
            }
            return dt;


        }
        public DataTable Buscar(string valor)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("buscar_materias", conexion);
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
        public string Insertar(Materias obj)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("insertar_materia", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar)).Value = obj.Nombre;
                    command.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.VarChar)).Value = obj.Descripcion;
                    command.Parameters.Add(new SqlParameter("@Creditos", SqlDbType.VarChar)).Value = obj.Creditos;
                   
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
        public string Actualizar(Materias obj)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("Actualizar_materia", conexion); 
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@IdMaterias", SqlDbType.Int)).Value = obj.IdMaterias;
                    command.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 100)).Value = obj.Nombre;
                    command.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.NVarChar)).Value = obj.Descripcion;
                    command.Parameters.Add(new SqlParameter("@Creditos", SqlDbType.Int)).Value = obj.Creditos;

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

        public string Eliminar(int IdMateria)
        {
            string respuesta = "";
            using (SqlConnection conexion = new Conexion().ObtenerConexion())
            {
                try
                {
                    SqlCommand command = new SqlCommand("eliminar_materia", conexion);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IdMaterias", SqlDbType.Int)).Value = IdMateria;
                    conexion.Open();
                    respuesta = command.ExecuteNonQuery() == 1 ? "OK" : "No se pudo eliminarr el registro";
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
