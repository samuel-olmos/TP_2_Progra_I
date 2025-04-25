using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Biblioteca
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Tema> ListaTemas { get; set; }
        public Biblioteca() 
        {
            ListaTemas = new List<Tema>();
        }
        public Biblioteca(int id, string nombre) 
        {
            Id = id;
            Nombre = nombre;
            ListaTemas = new List<Tema>();
        }

    }
}
