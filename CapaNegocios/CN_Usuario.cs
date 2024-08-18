using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();

        public bool IniciarSesion(string nombre, string clave, out string mensaje)
        {
            return objcd_usuario.IniciarSesion(nombre, clave, out mensaje);
        }
    }
}
