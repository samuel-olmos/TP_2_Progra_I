using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } //Opcional
        public List<Book> Books_List{ get; set; }
        public Topic()
        {
            Books_List = new List<Book>();
        }
        public Topic(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Books_List = new List<Book>();
        }
    }
}
