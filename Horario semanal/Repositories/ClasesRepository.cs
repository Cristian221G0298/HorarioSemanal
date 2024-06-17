using Horario_semanal.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horario_semanal.Repositories
{
    public class ClasesRepository
    {
        SQLiteConnection conexion;

        public ClasesRepository()
        {
            conexion = new("HorarioClases.sqlite");
            conexion.CreateTable<ClasesModel>();
        }

        public IEnumerable<ClasesModel> GetAll()
        {
            var datos = conexion.Table<ClasesModel>().OrderBy(x => x.Dia).ThenBy(x => x.Hora).ToList();
            return datos;
        }

        public void Insert(ClasesModel Clase)
        {
            conexion.Insert(Clase);
        }

        public void Delete(ClasesModel Clase)
        {
            conexion.Delete(Clase);
        }

        public void Update(ClasesModel Clase)
        {
            conexion.Update(Clase);
        }
    }
}
