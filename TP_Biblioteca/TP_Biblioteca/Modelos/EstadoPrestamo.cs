using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class EstadoPrestamo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public EstadoPrestamo() { }
        public EstadoPrestamo(int id, string nombre) 
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
