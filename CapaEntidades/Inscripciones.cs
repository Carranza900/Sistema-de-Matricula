using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Inscripciones
    {
        public int InscripcionId { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; }
        public Alumno oEstudiante { get; set; }
        public Materias oMaterias { get; set; }
    }
}
