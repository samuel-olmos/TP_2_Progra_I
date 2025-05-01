using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public Usuario() { }
        public Usuario(int id, string nombre, string apellido, string email)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
        }
    }
}
