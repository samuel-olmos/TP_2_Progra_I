using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Writer { get; set; } //DataType User?
        public string Prologue { get; set; } //Opcional
        public Topic Topic { get; set; } //Debería ser un atributo del libro? O que exista un List<Book> en Topic?

        public Book() { }

        public Book(int id, string name, string writer, string prologue, Topic topic)
        {
            Id = id;
            Name = name;
            Writer = writer;
            Prologue = prologue;
            Topic = topic;
        }
    }
}
