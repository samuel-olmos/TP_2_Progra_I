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
        public string Autor { get; set; } //DataType User?
        public string Prologo { get; set; } //Opcional
        //public Tema Tema { get; set; }

        public Libro() { }

        public Libro(int id, string nombre, string autor, string prologo)
        {
            Id = id;
            Nombre = nombre;
            Autor = autor;
            Prologo = prologo;
        }
    }
}
