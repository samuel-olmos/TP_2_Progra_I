using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books_List { get; set; }
        public Library() 
        {
            Books_List = new List<Book>();
        }
        public Library(int id, string name) 
        {
            Id = id;
            Name = name;
            Books_List = new List<Book>();
        }

    }
}
