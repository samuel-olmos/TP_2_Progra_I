using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Modelos
{
    internal class Tema
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }

        public Tema()
        {
            Libros = new List<Libro>();
        }

        public Tema(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            Libros = new List<Libro>();
        }
    }
}
