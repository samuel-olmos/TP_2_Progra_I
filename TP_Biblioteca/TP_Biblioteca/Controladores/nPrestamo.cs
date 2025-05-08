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
        // Men� principal
        public static void Menu()
        {
            string[] nombres = Program.Prestamos.Select(p => p.Libro.Nombre + " | " + p.Usuario.Nombre + " " + p.Usuario.Apellido + " | " + p.FechaPrestamo + " | " + p.Estado).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            int opcion;

            do
            {
                Console.Clear();
                opcion = Selection_Menu.Print("Pr�stamos", 0, opciones) + 1;

                switch (opcion)
                {
                    case 1: Agregar(); break;
                    case 2: /*Modificar()*/ break;
                    case 3: /*Eliminar()*/ break;
                    case 4: Listar(); break;
                    case 5: return;
                }
            } while (opcion != 5);
        }

        // Agregar pr�stamo
        public static void Agregar()
        {
            Console.Clear();

            // Verificar si hay libros disponibles
            var librosDisponibles = nLibro.VerificarDisponibilidad();
            if (librosDisponibles.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay libros disponibles para pr�stamo.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Crear un nuevo pr�stamo
            Prestamo prestamo = new Prestamo();

            // Asignar ID
            prestamo.Id = MaximoId();

            // Seleccionar libro
            prestamo.Libro = nLibro.ListarDisponibles();

            // Seleccionar usuario
            // TODO: Necesito la funci�n de listar en el controlador de usuarios
            // NOTE: Habr�a que ver si un usuario con pr�stamos activos/vencidos puede pedir otro libro o tiene un l�mite
            prestamo.Usuario = Program.Usuarios[Selection_Menu.Print("Seleccionar Usuario", 0, Program.Usuarios.Select(u => u.Nombre + " " + u.Apellido).ToArray())];
            //prestamo.Usuario = nUsuario.Listar();

            // Establecer fecha de pr�stamo como la fecha actual
            prestamo.FechaPrestamo = DateTime.Now;

            // La fecha l�mite de devoluci�n por default es 14 d�as (se puede extender al modificar el pr�stamo)

            // Agregar el pr�stamo a la lista
            Program.Prestamos.Add(prestamo);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nPr�stamo agregado con �xito.");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        // Men� listar
        public static void Listar()
        {
            string[] opciones = new string[] {
                "Filtrar por Estado",
                "Filtrar por Tema",
                "Filtrar por Usuario",
                "Mostrar Todos",
                "Volver"
            };

            int opcion;
            do
            {
                Console.Clear();
                opcion = Selection_Menu.Print("Listar Pr�stamos", 0, opciones) + 1;

                switch (opcion)
                {
                    case 1:
                        FiltrarPorEstado();
                        break;
                    case 2:
                        FiltrarPorTema();
                        break;
                    case 3:
                        FiltrarPorUsuario();
                        break;
                    case 4:
                        MostrarPrestamos(Program.Prestamos, "Todos los Pr�stamos");
                        break;
                    case 5:
                        return;
               }
            } while (opcion != 5);
        }

        // Mostrar pr�stamos
        private static void MostrarPrestamos(List<Prestamo> prestamos, string titulo)
        {
            Console.Clear();

            if (prestamos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se encontraron pr�stamos.");
                Console.ResetColor();
                return;
            }

            prestamos = Ordenar(prestamos);

            string[] prestamosInfo = prestamos.Select(p =>
                $"{p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
            ).ToArray();

            int seleccionado = Selection_Menu.Print(titulo, 0, prestamosInfo);

            MostrarDetallePrestamo(prestamos[seleccionado]);
        }

        // Mostrar detalles del pr�stamo
        private static void MostrarDetallePrestamo(Prestamo p)
        {
            Console.Clear();
            Console.WriteLine("=== Detalle del Pr�stamo ===\n");
            Console.WriteLine($"Libro: {p.Libro.Nombre}");
            Console.WriteLine($"Autor: {p.Libro.Autor}");
            Console.WriteLine($"Usuario: {p.Usuario.Nombre} {p.Usuario.Apellido}");
            Console.WriteLine($"Fecha de pr�stamo: {p.FechaPrestamo:dd/MM/yyyy}");
            Console.WriteLine($"Fecha l�mite: {p.FechaLimiteDevolucion:dd/MM/yyyy}");
            Console.WriteLine($"Fecha de devoluci�n: {(p.FechaDevolucionReal.HasValue ? p.FechaDevolucionReal.Value.ToString("dd/MM/yyyy") : "No devuelto")}");
            Console.WriteLine($"Estado: {p.Estado}");

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey(true);
        }

        // Filtrar por estado
        private static void FiltrarPorEstado()
        {
            Console.Clear();
            string[] estados = Enum.GetNames(typeof(EstadoPrestamo)); // Obtener nombres de los estados
            int estadoSeleccionado = Selection_Menu.Print("Seleccione el Estado", 0, estados) + 1;

            EstadoPrestamo estado = (EstadoPrestamo)estadoSeleccionado;
            var prestamosFiltrados = Program.Prestamos.Where(p => p.Estado == estado).ToList();

            MostrarPrestamos(prestamosFiltrados, $"Pr�stamos con estado: {estado}");
        }

        // Filtrar por tema
        private static void FiltrarPorTema()
        {
            Console.Clear();
            // Obtener temas �nicos de los libros
            var temas = Program.Libros.SelectMany(l => l.Temas).Distinct().ToArray();

            // Verificar si hay temas
            if (temas.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay temas disponibles.");
                Console.ResetColor();
                return;
            }

            string[] temasNombres = temas.Select(t => t.Nombre).ToArray();

            // Mostrar lista de temas
            int temaSeleccionado = Selection_Menu.Print("Seleccione el Tema", 0, temasNombres);
            string tema = temasNombres[temaSeleccionado];

            var prestamosFiltrados = Program.Prestamos
                .Where(p => p.Libro.Temas.Any(t => t.Nombre == tema))
                .ToList();

            MostrarPrestamos(prestamosFiltrados, $"Pr�stamos de libros con tema: {tema}");
        }

        // Filtrar por usuario
        private static void FiltrarPorUsuario()
        {
            Console.Clear();
            // Crear lista de usuarios con pr�stamos
            var usuarios = Program.Usuarios
                .Select(u => $"{u.Nombre} {u.Apellido}")
                .ToArray();

            // Verificar si hay usuarios
            if (usuarios.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay usuarios disponibles.");
                Console.ResetColor();
                return;
            }

            // Mostrar lista de usuarios
            int usuarioSeleccionado = Selection_Menu.Print("Seleccione el Usuario", 0, usuarios);
            var usuario = Program.Usuarios[usuarioSeleccionado];

            var prestamosFiltrados = Program.Prestamos
                .Where(p => p.Usuario.Id == usuario.Id)
                .ToList();

            MostrarPrestamos(prestamosFiltrados, $"Pr�stamos del usuario: {usuario.Nombre} {usuario.Apellido}");
        }

        // Ordenar por fecha de pr�stamo (descendente)
        public static List<Prestamo> Ordenar(List<Prestamo> prestamos)
        {
            return prestamos.OrderByDescending(p => p.FechaPrestamo).ToList();
        }

        // Obtener el ID m�ximo
        public static int MaximoId()
        {
            int max = 0;
            foreach (Prestamo prestamo in Program.Prestamos) if (prestamo.Id > max) max = prestamo.Id;
            return max + 1;
        }
    }
}