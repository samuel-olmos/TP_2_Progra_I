using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Libro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Usuario Autor { get; set; } //DataType User?
        public string Prologo { get; set; } //Opcional

        public Libro() { }

        public Libro(int id, string nombre, Usuario autor, string prologo)
        {
            Id = id;
            Nombre = nombre;
            Autor = autor;
            Prologo = prologo;
        }
    }
}
