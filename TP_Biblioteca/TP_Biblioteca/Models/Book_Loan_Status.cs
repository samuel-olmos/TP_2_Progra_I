using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Book_Loan_Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Book_Loan_Status() { }
        public Book_Loan_Status(int id, string name) 
        {
            Id = id;
            Name = name;
        }
    }
}
