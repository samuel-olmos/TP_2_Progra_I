using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public User() { }
        public User(int id, string name, string last_name, string email)
        {
            Id = id;
            Name = name;
            Last_Name = last_name;
            Email = email;
        }
    }
}
