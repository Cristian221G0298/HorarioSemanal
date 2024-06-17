using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horario_semanal.Models
{
    public enum Dias { Lunes, Martes, Miércoles, Jueves,  Viernes, Sábado, Domingo }
    [Table("Actividades")]
    public class ActvidadesModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(30)]
        public string Nombre { get; set; } = null!;
        [NotNull]
        public Dias Dia { get; set; }
        [NotNull]
        public int Hora { get; set; }
        [NotNull, MaxLength(100)]
        public string Descripcion { get; set; } = null!;
    }
}
