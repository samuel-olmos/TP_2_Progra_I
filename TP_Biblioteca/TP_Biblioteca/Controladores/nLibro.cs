using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Biblioteca.Models;
using Mi_libreria;

namespace TP_Biblioteca.Drivers
{
    internal class nLibro
    {
        public static void Agregar()
        {
            Console.Clear();
            bool existe = false;
            Libro libro = new Libro();
            libro.Id = MaximoId();
            Console.Write("Coloque el nombre: ");
            libro.Nombre = Validations.Letters_only_input();
            Console.Write("Coloque el nombre del autor: ");
            libro.Autor = Validations.Letters_only_input();
            Console.Write("Coloque una descripción del libro: ");
            libro.Prologo = Validations.Letters_only_input();

            foreach (Libro l in Program.Libros)
            {
                if (l.Id == libro.Id && l.Nombre == libro.Nombre && l.Autor == libro.Autor && l.Prologo == libro.Prologo)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nEl libro que desea agregar ya existe");
                    Console.ResetColor();
                    existe = true;
                }
            }
            if (existe == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nEl libro se agregó con éxito");
                Console.ResetColor();
                Program.Libros.Add(libro);
            }
            Console.ReadKey(true);
        }

        public static void Ordenar()
        {
            Program.Libros = Program.Libros.OrderBy(l => l.Nombre).ToList();
        }

        public static void Listar()
        {
            Console.Clear();
            if (Program.Libros.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }
            string[] nombres = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
            Libro libro_seleccionado = Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)];
            Console.Clear();
            Console.WriteLine($"Nombre del libro: {libro_seleccionado.Nombre}");
            Console.WriteLine($"Autor del libro: {libro_seleccionado.Autor}");
            Console.WriteLine($"Descripción: {libro_seleccionado.Prologo}");
            Console.WriteLine("\nPresione cualquier tecla para volver");
            Console.ReadKey(true);
        }

        public static void Modificar(Libro libro)
        {
            string[] opciones = new string[] { "Cambiar nombre", "Cambiar Autor", "Cambiar Prólogo", "Salir" };
            int opcion = Selection_Menu.Print(libro.Nombre + " - By " + libro.Autor, 1, opciones);
            switch (opcion)
            {
                case 1: Console.Write("Ingrese Nuevo Nombre: "); libro.Nombre = Validations.Letters_only_input(); Modificar(libro); break;
                case 2: Console.Write("Ingrese Nuevo Apellido: "); libro.Autor = Validations.Letters_only_input(); Modificar(libro); break;
                case 3: Console.Write("Ingrese Nuevo Prólogo: "); libro.Prologo = Validations.Letters_only_input(); Modificar(libro); break;
                case 4: break;
                default: Modificar(libro); break;
            }
        }

        public static void Eliminar(Libro libro)
        {
            //Falta validar que no se pueda eliminar un libro si tiene un préstamo activo
            Program.Libros.Remove(libro);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nLibro borrado con éxito");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static int MaximoId()
        {
            int max = 0;
            foreach (Libro libro in Program.Libros) if (libro.Id > max) max = libro.Id;
            return max + 1;
        }

        public static void Menu()
        {
            string[] nombres = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Libros", 0, opciones)+1;
            switch (opcion)
            {
                case 1: Console.Clear(); Ordenar(); Agregar(); Menu(); break;
                case 2: Console.Clear(); Ordenar();
                    if (Program.Libros.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    Modificar(Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)]); Menu(); break;
                case 3: Console.Clear(); Ordenar();
                    if (Program.Libros.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    Eliminar(Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)]); Menu(); break;
                case 4: Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }
    }
}
