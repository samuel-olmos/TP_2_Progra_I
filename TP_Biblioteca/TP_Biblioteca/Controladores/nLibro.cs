using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Biblioteca.Modelos;
using Mi_libreria;

namespace TP_Biblioteca.Controladores
{
    internal class nLibro
    {
        public static void Agregar()
        {
            Console.Clear();
            bool existe = false;
            bool agregarTema = true;
            List<Tema> temasDelLibro = new List<Tema>();
            Libro libro = new Libro();
            libro.Id = MaximoId();
            Console.Write("Coloque el nombre: ");
            libro.Nombre = Validations.Letters_only_input();
            Console.Write("Coloque el nombre del autor: ");
            libro.Autor = Validations.Letters_only_input();
            Console.Write("Coloque una descripción del libro: ");
            libro.Prologo = Validations.Letters_only_input();
            libro.Temas = temasDelLibro;

            while (agregarTema)
            {
                string[] opciones = { "SI", "NO" };
                int opcion = Selection_Menu.Print("¿Desea asociar el libro con algún tema?", 0, opciones);
                switch (opcion)
                {
                    case 0:
                        Console.Clear(); nTema.Ordenar();
                        if (Program.Temas.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nNo existen datos");
                            Console.ResetColor();
                            Console.ReadKey(true);
                            Menu();
                            break;
                        }

                        //No se pueden ver los temas desde el método Listar
                        string[] nombres = Program.Temas.Select(t => t.Nombre).ToArray();
                        Tema temaPorAgregar = (Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombres)]);
                        foreach (Tema t in libro.Temas)
                        {
                            if (temaPorAgregar.Id == t.Id && temaPorAgregar.Nombre == t.Nombre)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("\nEl tema que desea asignar al libro ya está asignado");
                                Console.ResetColor();
                                Console.ReadKey(true);
                            }
                            else
                            {
                                libro.Temas.Add(temaPorAgregar);
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("\nEl tema se agregó con éxito");
                                Console.ResetColor();
                                Console.ReadKey(true);
                            }
                        }
                        agregarTema = true; break;
                    case 1: agregarTema = false; break;
                    default: agregarTema = true; break;
                }
            }

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
            Program.Libros = Program.Libros.OrderByDescending(l => l.Nombre).ToList();
        }

        public static void Listar()
        {
            Console.Clear();
            
            // Filtrar libros disponibles
            var librosDisponibles = Program.Libros.Where(l => l.Disponible).ToList();

            // Verificar que existan libros disponibles
            if (librosDisponibles.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen libros disponibles");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }

            string[] nombres = librosDisponibles.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
            Libro libro_seleccionado = librosDisponibles[Selection_Menu.Print("Lista de Libros", 0, nombres)];

            Console.Clear();
            Console.WriteLine($"Nombre del libro: {libro_seleccionado.Nombre}");
            Console.WriteLine($"Autor del libro: {libro_seleccionado.Autor}");
            Console.WriteLine($"Descripción: {libro_seleccionado.Prologo}");
            Console.Write("Tema/s: ");
            Console.WriteLine($"Lista de temas: {libro_seleccionado.Temas.Count}");
            for (int i = 0; i < libro_seleccionado.Temas.Count; i++)
            {
                if (i == libro_seleccionado.Temas.Count-1)
                {
                    Console.WriteLine($"{libro_seleccionado.Temas[i].Nombre}");
                }
                else
                {
                    Console.Write($"{libro_seleccionado.Temas[i].Nombre}, ");
                }
            }
            Console.WriteLine();
            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey(true);
        }

        // Verificar disponibilidad de libros
        public static List<Libro> VerificarDisponibilidad()
        {
            // Filtrar libros disponibles que no están en préstamos activos o vencidos
            return Program.Libros
                .Where(l => l.Disponible && !Program.Prestamos.Any(p =>
                    p.Libro.Id == l.Id &&
                    (p.Estado == EstadoPrestamo.Activo || p.Estado == EstadoPrestamo.Vencido)))
                .ToList();
        }

        // Listar libros disponibles para préstamo
        public static Libro ListarDisponibles()
        {
            Console.Clear();

            // Obtener libros disponibles usando la nueva función
            var librosDisponibles = VerificarDisponibilidad();

            // Verificar si hay libros disponibles
            if (librosDisponibles.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay libros disponibles para préstamo.");
                Console.ResetColor();
                return null;
            }

            // Crear un array con los nombres de los libros disponibles
            string[] nombresLibros = librosDisponibles.Select(l => l.Nombre).ToArray();

            // Mostrar lista de libros disponibles
            int seleccionado = Selection_Menu.Print("Libros Disponibles", 0, nombresLibros);

            // Retornar el libro seleccionado
            return librosDisponibles[seleccionado];
        }

        // Modificar libro
        public static void Modificar(Libro libro)
        {
            string[] opciones = { "Cambiar nombre", "Cambiar Autor", "Cambiar Prólogo", "Salir" };
            int opcion = Selection_Menu.Print(libro.Nombre + " - By " + libro.Autor, 0, opciones);
            switch (opcion)
            {
                case 0: Console.Write("Ingrese Nuevo Nombre: "); libro.Nombre = Validations.Letters_only_input(); Modificar(libro); break;
                case 1: Console.Write("Ingrese Nuevo Apellido: "); libro.Autor = Validations.Letters_only_input(); Modificar(libro); break;
                case 2: Console.Write("Ingrese Nuevo Prólogo: "); libro.Prologo = Validations.Letters_only_input(); Modificar(libro); break;
                case 3: break;
                default: Modificar(libro); break;
            }
        }

        // Eliminar libro
        public static void Eliminar(Libro libro)
        {
            // Verificar si el libro está disponible
            var librosDisponibles = VerificarDisponibilidad();

            if (!librosDisponibles.Contains(libro))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se puede eliminar el libro porque está prestado (Activo o Vencido).");
                Console.ResetColor();
            }
            else
            {
                libro.Disponible = false;
                Program.Libros.Remove(libro);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nLibro borrado con éxito.");
                Console.ResetColor();
            }

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
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Libros", 0, opciones);
            switch (opcion)
            {
                case 0: Console.Clear(); Ordenar(); Agregar(); Menu(); break;
                case 1: Console.Clear(); Ordenar();
                    if (Program.Libros.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    string[] nombresModificar = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                    Modificar(Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombresModificar)]); Menu(); break;
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
                    string[] nombresEliminar = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                    Eliminar(Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombresEliminar)]); Menu(); break;
                case 3: Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 4: break;
                default: Menu(); break;
            }
        }
    }
}
