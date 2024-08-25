using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();

        
        public bool IniciarSesion(string usuario, string contrasena, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(usuario))
            {
                mensaje += "Es necesario ingresar el nombre de usuario.\n";
            }

            if (string.IsNullOrEmpty(contrasena))
            {
                mensaje += "Es necesario ingresar la contraseña.\n";
            }

            if (mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_usuario.IniciarSesion(usuario, contrasena, out mensaje);
            }
        }

        public static DataTable Listar()
        {
            CD_Usuario datos = new CD_Usuario();
            return datos.listar();
        }
        public static DataTable Buscar(string valor)
        {
            CD_Usuario datos = new CD_Usuario();
            return datos.Buscar(valor);
        }

        public static string Insertar(string Nombre, string Apellido, string NombreUsuario, string Contraseña, string Email, int Estado, string Rol)
        {
            CD_Usuario datos = new CD_Usuario();
            Usuario obj = new Usuario
            {
                Nombre = Nombre,
                Apellido = Apellido,
                NombreUsuario = NombreUsuario,
                Contraseña = Contraseña,
                Email = Email,
                Estado = Estado,
                Rol = Rol
            };
            return datos.Insertar(obj);
        }

        public static string Actualizar(int IdUsuario, string Nombre, string Apellido, string NombreUsuario, string Contraseña, string Email, int Estado, string Rol)
        {
            CD_Usuario datos = new CD_Usuario();
            Usuario obj = new Usuario
            {
                IdUsuario = IdUsuario,
                Nombre = Nombre,
                Apellido = Apellido,
                NombreUsuario = NombreUsuario,
                Contraseña = Contraseña,
                Email = Email,
                Estado = Estado,
                Rol = Rol,
            };
            return datos.Actualizar(obj);
        }
        public static string Eliminar(int IdUsuario)
        {
            CD_Usuario datos = new CD_Usuario();
            return datos.Eliminar(IdUsuario);
        }

    }
}


