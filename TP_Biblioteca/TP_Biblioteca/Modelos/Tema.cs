using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Tema
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } //Opcional
        public List<Libro> ListaLibros{ get; set; }
        public Tema()
        {
            ListaLibros = new List<Libro>();
        }
        public Tema(int id, string nombre, string descripcion)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            ListaLibros = new List<Libro>();
        }
    }
}
