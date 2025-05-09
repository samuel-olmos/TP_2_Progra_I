using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Modelos
{
    internal class Libro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Autor { get; set; }
        public string Prologo { get; set; }
        public List<Tema> Temas { get; set; }
        public bool Activo { get; set; } = true; // En existencia

        public Libro()
        {
            Temas = new List<Tema>();
        }

        public Libro(int id, string nombre, string autor, string prologo)
        {
            Id = id;
            Nombre = nombre;
            Autor = autor;
            Prologo = prologo;
            Temas = new List<Tema>();
        }
    }
}
