using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Prestamo
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Libro Libro { get; set; }
        public Usuario Usuario { get; set; }
        public EstadoPrestamo Estado { get; set; }
        public Prestamo() { }
        public Prestamo(int id, DateTime fecha, Libro libro, Usuario usuario, EstadoPrestamo estado)
        {
            Id = id;
            Fecha = fecha;
            Libro = libro;
            Usuario = usuario;
            Estado = estado;
        }
    }
}
