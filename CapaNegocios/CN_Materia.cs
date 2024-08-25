using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using System.Data;
using CapaEntidades;

namespace CapaNegocios
{
    public class CN_Materia
    {
        public static DataTable Listar()
        {
            Datos_Materia datos = new Datos_Materia();
            return datos.Listar();
        }

        public static DataTable Buscar(string valor)
        {
            Datos_Materia datos = new Datos_Materia();
            return datos.Buscar(valor);
        }

        public static string Insertar(string Nombre, string Descripcion,int Creditos)
        {
            Datos_Materia datos = new Datos_Materia();
            Materias obj = new Materias
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Creditos = Creditos,
            };
            return datos.Insertar(obj);
        }

        public static string Actualizar(int IdMaterias, string Nombre, string Descripcion, int Creditos)
        {
            Datos_Materia datos = new Datos_Materia();
            Materias obj = new Materias
            {
                IdMaterias = IdMaterias,
                Nombre = Nombre,
                Descripcion = Descripcion,
                Creditos = Creditos,
            };
            return datos.Actualizar(obj);

        }

        public static string Eliminar(int IdMaterias)
        {
            Datos_Materia datos = new Datos_Materia();
            return datos.Eliminar(IdMaterias);
        }
    }
    
}
