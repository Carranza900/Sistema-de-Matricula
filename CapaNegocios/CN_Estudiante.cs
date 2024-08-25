using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidades;
using DocumentFormat.OpenXml.Spreadsheet;
namespace CapaNegocios
{
    public class CN_Estudiante
    {
        public static DataTable listar()
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.listar();
        }

       /* public static DataTable ListarMaterias()
        {
            Datos_Materia datos = new Datos_Materia();
            return datos.Listar();
        }*/


        public static DataTable Buscar(string valor)
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.Buscar(valor);
        }

        public static string Insertar(string nombre, string apellido, DateTime fechaNacimiento, string telefono, int estado, int idMateria)
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.Insertar(nombre, apellido, fechaNacimiento, telefono, estado, idMateria);
        }


        public static string Actualizar(int Id, string Nombre, string Apellido, DateTime FechaNacimiento, string Telefono, int Estado, int? IdMateria)
        {
            Datos_Alumno datos = new Datos_Alumno();
            Alumno obj = new Alumno
            {
                IdEstudiante = Id,
                Nombre = Nombre,
                Apellido = Apellido,
                FechaNacimiento = FechaNacimiento,
                Telefono = Telefono,
                Estado = Estado,
                IdMateria = IdMateria
            };
            return datos.Actualizar(obj);
        }

        public static string Eliminar(int Id)
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.Eliminar(Id);
        }
        public static string Activar(int Id)
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.Activar(Id);
        }
        public static string Desactivar(int Id)
        {
            Datos_Alumno datos = new Datos_Alumno();
            return datos.Desactivar(Id);
        }
    }
}
