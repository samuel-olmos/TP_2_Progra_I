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
            // Selección de opciones
            string[] opciones = new string[] { "Filtrar por Estado", "Filtrar por Tema", "Filtrar por Usuario", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Listar Préstamos", 0, opciones) + 1;
            switch(opcion)
            {
                // Filtrar por Estado
                case 1:
                    Console.Clear();
                    string[] estados = new string[] { "Activo", "Vencido", "Devuelto" };
                    int estadoSeleccionado = Selection_Menu.Print("Filtrar por Estado", 0, estados) + 1;

                    // Convertir la selección a EstadoPrestamo
                    EstadoPrestamo estado = (EstadoPrestamo)estadoSeleccionado;

                    // Llamar a la función para filtrar
                    var prestamosFiltrados = FiltrarPorEstado(estado);

                    // Mostrar los préstamos filtrados
                    if (prestamosFiltrados.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo se encontraron préstamos con el estado seleccionado.");
                        Console.ResetColor();
                    }
                    else
                    {
                        string[] prestamosInfo = prestamosFiltrados.Select(p =>
                            $"{p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Estado}"
                        ).ToArray();

                        Selection_Menu.Print("Préstamos Filtrados", 0, prestamosInfo);
                    }

                    Console.ReadKey(true);
                    return;
                // Filtrar por Tema
                case 2:
                    Console.Clear();
                    // Implementar lógica para filtrar por tema
                    break;
                // Filtrar por Usuario
                case 3:
                    Console.Clear();
                    // Implementar lógica para filtrar por usuario
                    break;
                // Volver
                case 4: return;
            }


            /*
            Console.Clear();
            // Verificar que existan préstamos
            if (Program.Prestamos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }

            string[] nombres = Program.Prestamos.Select(p => p.Libro.Nombre + " | " + p.Usuario.Nombre + " " + p.Usuario.Apellido + " | " + p.FechaPrestamo +  " | " + p.Estado).ToArray();
            Prestamo prestamoSeleccionado = Program.Prestamos[Selection_Menu.Print("Lista de Préstamos", 0, nombres)];
            Console.Clear();
            Console.WriteLine($"INFO ACA");
            Console.WriteLine("\nPresione cualquier tecla para volver");
            Console.ReadKey(true);
            */
        }

        public static void Menu()
        {
            string[] nombres = Program.Prestamos.Select(p => p.Libro.Nombre + " | " + p.Usuario.Nombre + " " + p.Usuario.Apellido + " | " + p.FechaPrestamo + " | " + p.Estado).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Préstamos", 0, opciones) + 1;
            switch (opcion)
            {
                // Agregar
                case 1: Console.Clear(); Ordenar(); /*Agregar();*/ Menu(); break;

                // Modificar
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

                // Eliminar
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

                // Listar
                case 4:
                    Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }

        // Ordenar por fecha
        public static void Ordenar()
        {
            Program.Prestamos = Program.Prestamos.OrderBy(p => p.FechaPrestamo).ToList();
        }

        public static List<Prestamo> FiltrarPorEstado(EstadoPrestamo estado)
        {
            // Filtrar los préstamos por el estado especificado
            return Program.Prestamos.Where(p => p.Estado == estado).ToList();
        }


        public static int MaximoId()
        {
            int max = 0;
            foreach (Prestamo prestamo in Program.Prestamos) if (prestamo.Id > max) max = prestamo.Id;
            return max + 1;
        }
    }
}