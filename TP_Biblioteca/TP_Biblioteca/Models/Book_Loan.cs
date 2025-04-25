using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Models
{
    internal class Book_Loan
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Book Book_To_Lend { get; set; }
        public User Loan_User { get; set; }
        public Book_Loan_Status Status { get; set; }
        public Book_Loan() { }
        public Book_Loan(int id, DateTime date, Book book_To_Lend, User loan_User, Book_Loan_Status status)
        {
            Id = id;
            Date = date;
            Book_To_Lend = book_To_Lend;
            Loan_User = loan_User;
            Status = status;
        }
    }
}
