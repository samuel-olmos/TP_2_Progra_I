using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Modelos
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public List<Prestamo> Prestamos { get; set; }
        public bool Activo { get; set; } = true;

        public Usuario() {
            Prestamos = new List<Prestamo>();
        }

        public Usuario(int id, string nombre, string apellido, string email)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Prestamos = new List<Prestamo>();
        }
    }
}
