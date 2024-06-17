using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Horario_semanal.Models
{
    [Table("Clases")]
    public class ClasesModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public Dias Dia { get; set; }
        [NotNull]
        public int Hora { get; set; }
        [NotNull, MaxLength(40)]
        public string Asignatura { get; set; } = null!;
        [NotNull, MaxLength(60)]
        public string Maestro { get; set; } = null!;
        [NotNull, MaxLength(20)]
        public string Aula { get; set; } = null!;
    }
}
