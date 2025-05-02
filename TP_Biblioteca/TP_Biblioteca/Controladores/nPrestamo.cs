using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi_libreria;
using TP_Biblioteca.Controladores;
using TP_Biblioteca.Modelos;

namespace TP_Biblioteca.Controladores
{
    internal class nPrestamo
    {
        public static void Listar()
        {
            Console.Clear();
            if (Program.Prestamos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }
            string[] nombres = Program.Prestamos.Select(p => "ID: " + p.Id + " | " + p.Usuario.Nombre + " " + p.Usuario.Apellido + " | " + p.Libro.Nombre + " | " + p.Estado).ToArray();
            Prestamo prestamoSeleccionado = Program.Prestamos[Selection_Menu.Print("Lista de Préstamos", 0, nombres)];
            Console.Clear();
            Console.WriteLine($"INFO ACA");
            Console.WriteLine("\nPresione cualquier tecla para volver");
            Console.ReadKey(true);
        }

        public static void Menu()
        {
            string[] nombres = Program.Prestamos.Select(p => "ID: " + p.Id + " | " + p.Usuario.Nombre + " " + p.Usuario.Apellido + " | " + p.Libro.Nombre + " | " + p.Estado).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Préstamos", 0, opciones) + 1;
            switch (opcion)
            {
                case 1: Console.Clear(); Ordenar(); /*Agregar();*/ Menu(); break;
                case 2:
                    Console.Clear(); Ordenar();
                    if (Program.Prestamos.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                /*Modificar(Program.Prestamos[Selection_Menu.Print("Lista de Préstamos", 0, nombres)]); Menu(); */ break;
                case 3:
                    Console.Clear(); Ordenar();
                    if (Program.Prestamos.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                /*Eliminar(Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)]); Menu();*/ break;
                case 4: Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }

        // Ver si lo hago por ID o por fecha
        public static void Ordenar()
        {
            Program.Prestamos = Program.Prestamos.OrderBy(p => p.Id).ToList();
        }

        public static int MaximoId()
        {
            int max = 0;
            foreach (Prestamo prestamo in Program.Prestamos) if (prestamo.Id > max) max = prestamo.Id;
            return max + 1;
        }
    }
}