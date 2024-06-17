using Horario_semanal.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horario_semanal.Repositories
{
    public class ActividadesRepository
    {
        SQLiteConnection conexion;

        public ActividadesRepository()
        {
            conexion = new("HorarioActividades.sqlite");
            conexion.CreateTable<ActvidadesModel>();
        }

        public IEnumerable<ActvidadesModel> GetAll()
        {
            var datos = conexion.Table<ActvidadesModel>().OrderBy(x => x.Dia).ThenBy(x => x.Hora).ToList();
            return datos;
        }

        public void Insert(ActvidadesModel Actividad)
        {
            conexion.Insert(Actividad);
        }

        public void Delete(ActvidadesModel Actividad)
        {
            conexion.Delete(Actividad);
        }

        public void Update(ActvidadesModel Actividad)
        {
            conexion.Update(Actividad);
        }
    }
}
