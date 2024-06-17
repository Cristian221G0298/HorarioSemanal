using GalaSoft.MvvmLight.Command;
using Horario_semanal.Models;
using Horario_semanal.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Horario_semanal.ViewModels
{
    public enum VistasClases { Lista, Agregar, Editar, Eliminar, Detalles, ListaA, AgregarA, EliminarA, EditarA, DetallesA }
    public class ClasesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ClasesModel> Clases { get; set; } = new();
        public ObservableCollection<ActvidadesModel> Actividades { get; set; } = new();
        public ClasesModel Clase { get; set; } = null!;
        public ActvidadesModel Actividad { get; set; } = null!;
        public ActividadesRepository RepositorioActividades = new();
        public ClasesRepository RepositorioClases = new();
        public VistasClases Vista {  get; set; }
        public string Error { get; set; } = "";

        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand {  get; set; }
        public ICommand CambiarVistaCommand {  get; set; }

        public ClasesViewModel()
        {
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<VistasClases>(CambiarVista);

            AgregarLista();
            AgregarListaA();
        }

        void Actualizar(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        ClasesModel claseClon = null!;
        ActvidadesModel actClon = null!;
        void CambiarVista(VistasClases vista)
        {
            if(vista == VistasClases.Agregar)
            {
                Clase = new();
            }
            if (vista == VistasClases.AgregarA)
            {
                Actividad = new();
            }

            if (vista == VistasClases.Editar && Clase != null)
            {
                var clon = new ClasesModel()
                {
                    Id = Clase.Id,
                    Dia = Clase.Dia,
                    Hora = Clase.Hora,
                    Asignatura = Clase.Asignatura,
                    Maestro = Clase.Maestro,
                    Aula = Clase.Aula
                };
                Clase = clon;
                claseClon = clon;
            }
            if (vista == VistasClases.EditarA && Actividad != null)
            {
                var clon = new ActvidadesModel()
                {
                    Id = Actividad.Id,
                    Dia = Actividad.Dia,
                    Hora = Actividad.Hora,
                    Nombre = Actividad.Nombre,
                    Descripcion = Actividad.Descripcion,
                };
                Actividad = clon;
                actClon = clon;
            }

            if (vista == VistasClases.Lista)
            {
                Clase = null;
            }
            if (vista == VistasClases.ListaA)
            {
                Actividad = null;
            }

            if(Clase == null && Actividad == null)
            {
                if ((vista == VistasClases.Detalles || vista == VistasClases.Editar || vista == VistasClases.Eliminar) && Clase == null)
                {
                    vista = VistasClases.Lista;
                }
                if ((vista == VistasClases.DetallesA || vista == VistasClases.EditarA || vista == VistasClases.EliminarA) && Actividad == null)
                {
                    vista = VistasClases.ListaA;
                }
            } 

            Error = "";
            Vista = vista;
            Actualizar();
        }
        void Agregar()
        {
            if(Clase != null)
            {
                Error = "";
                if (RepositorioClases.GetAll().Any(x => x.Dia == Clase.Dia && x.Hora == Clase.Hora) || RepositorioActividades.GetAll().Any(x => x.Dia == Clase.Dia && x.Hora == Clase.Hora))
                {
                    Error += "- Ya hay una actividad en este horario\n";
                }
                if (string.IsNullOrWhiteSpace(Clase.Asignatura) || string.IsNullOrWhiteSpace(Clase.Maestro) || string.IsNullOrWhiteSpace(Clase.Aula))
                {
                    Error += "- Ningún campo debe ir vacío\n";
                }
                if (Clase.Hora < 0 || Clase.Hora > 23)
                {
                    Error += "- El horario está establecido en un formato de 24 horas (00 - 23)\n";
                }
                Actualizar(nameof(Error));
                if (Error == "")
                {
                    RepositorioClases.Insert(Clase);
                    AgregarLista();
                    CambiarVista(VistasClases.Lista);
                }
            }

            if (Actividad != null)
            {
                Error = "";
                if (RepositorioActividades.GetAll().Any(x => x.Dia == Actividad.Dia && x.Hora == Actividad.Hora) || RepositorioClases.GetAll().Any(x => x.Dia == Actividad.Dia && x.Hora == Actividad.Hora))
                {
                    Error += "- Ya hay una actividad en este horario\n";
                }
                if (string.IsNullOrWhiteSpace(Actividad.Nombre) || string.IsNullOrWhiteSpace(Actividad.Descripcion))
                {
                    Error += "- Ningún campo debe ir vacío\n";
                }
                if (Actividad.Hora < 0 || Actividad.Hora > 23)
                {
                    Error += "- El horario está establecido en un formato de 24 horas (00 - 23)\n";
                }
                Actualizar(nameof(Error));
                if (Error == "")
                {
                    RepositorioActividades.Insert(Actividad);
                    AgregarListaA();
                    CambiarVista(VistasClases.ListaA);
                }
            }
        }
        void AgregarLista()
        {
            Clases.Clear();
            foreach (var item in RepositorioClases.GetAll().ToList())
            {
                Clases.Add(item);
            }
        }
        void AgregarListaA()
        {
            Actividades.Clear();
            foreach (var item in RepositorioActividades.GetAll().ToList())
            {
                Actividades.Add(item);
            }
        }

        void Editar()
        {
            if(Clase != null)
            {
                Error = "";
                if (claseClon.Dia != Clase.Dia && claseClon.Hora != Clase.Hora)
                {
                    if (RepositorioClases.GetAll().Any(x => x.Dia == Clase.Dia && x.Hora == Clase.Hora) || RepositorioActividades.GetAll().Any(x => x.Dia == Clase.Dia && x.Hora == Clase.Hora))
                    {
                        Error += "- Ya hay una actividad en este horario\n";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Asignatura) || string.IsNullOrWhiteSpace(Clase.Maestro) || string.IsNullOrWhiteSpace(Clase.Aula))
                    {
                        Error += "- Ningún campo debe ir vacío";
                    }
                }
                Actualizar(nameof(Error));

                if (Error == "")
                {
                    RepositorioClases.Update(Clase);
                    AgregarLista();
                    CambiarVista(VistasClases.Lista);
                }
            }
            if (Actividad != null)
            {
                Error = "";
                if (actClon.Dia != Actividad.Dia && actClon.Hora != Actividad.Hora)
                {
                    if (RepositorioActividades.GetAll().Any(x => x.Dia == Actividad.Dia && x.Hora == Actividad.Hora) || RepositorioClases.GetAll().Any(x => x.Dia == Actividad.Dia && x.Hora == Actividad.Hora))
                    {
                        Error += "- Ya hay una actividad en este horario\n";
                    }
                    if (string.IsNullOrWhiteSpace(Actividad.Nombre) || string.IsNullOrWhiteSpace(Actividad.Descripcion))
                    {
                        Error += "- Ningún campo debe ir vacío";
                    }
                }
                Actualizar(nameof(Error));

                if (Error == "")
                {
                    RepositorioActividades.Update(Actividad);
                    AgregarListaA();
                    CambiarVista(VistasClases.ListaA);
                }
            }
        }
        void Eliminar()
        {
            if(Clase != null)
            {
                RepositorioClases.Delete(Clase);
                AgregarLista();
                CambiarVista(VistasClases.Lista);
            }
            if (Actividad != null)
            {
                RepositorioActividades.Delete(Actividad);
                AgregarListaA();
                CambiarVista(VistasClases.ListaA);
            }
        }
    }
}
