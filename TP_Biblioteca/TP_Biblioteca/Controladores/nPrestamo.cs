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
        // Menú principal
        public static void Menu()
        {
            string[] nombres = Program.Prestamos
                .Where(p => p.Activo)  // Filtrar solo préstamos activos
                .Select(p => $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}")
                .ToArray();
            string[] opciones = { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            int opcion;

            do
            {
                Console.Clear();
                opcion = Selection_Menu.Print("Préstamos", 0, opciones);

                switch (opcion)
                {
                    case 0: Agregar(); break;
                    case 1: Modificar(); break;
                    case 2: Eliminar(); break;
                    case 3: Listar(); break;
                    case 4: return;
                }
            } while (opcion != 4);
        }

        // Agregar préstamo
        public static void Agregar()
        {
            Console.Clear();

            // Crear un nuevo préstamo
            Prestamo prestamo = new Prestamo();

            // Asignar ID
            prestamo.Id = MaximoId();

            // Preguntar primero por la fecha del préstamo
            string[] opcionFecha = { "Fecha actual", "Fecha personalizada" };
            int fechaSeleccionada = Selection_Menu.Print("Seleccione la fecha del préstamo", 0, opcionFecha);

            DateTime fechaActual = DateTime.Now;

            if (fechaSeleccionada == 0) // Fecha actual
            {
                prestamo.FechaPrestamo = fechaActual;

                // Para préstamos actuales, mostrar solo libros actualmente disponibles
                var librosDisponibles = nLibro.VerificarDisponibilidad();
                if (librosDisponibles.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nNo hay libros disponibles para préstamo en este momento.");
                    Console.ResetColor();
                    Console.WriteLine("\nPresione cualquier tecla para volver...");
                    Console.ReadKey(true);
                    return;
                }

                string[] nombresLibros = librosDisponibles.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                int libroSeleccionado = Selection_Menu.Print("Seleccione un libro disponible", 0, nombresLibros);
                prestamo.Libro = librosDisponibles[libroSeleccionado];
            }
            else // Fecha personalizada
            {
                // Obtener fecha personalizada
                prestamo.FechaPrestamo = Validations.Date_input("Ingrese la fecha de préstamo (dd/MM/yyyy):");

                // Seleccionar libro de todos los libros no eliminados
                prestamo.Libro = nLibro.SeleccionarLibro("Seleccione un libro para el préstamo");

                // Si no se seleccionó ningún libro, cancelar
                if (prestamo.Libro == null) return;

                // Calcular fecha límite de devolución (14 días por defecto)
                DateTime fechaLimite = prestamo.FechaPrestamo.AddDays(14);

                // Verificar si el libro está disponible en esas fechas
                bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(prestamo.Libro, prestamo.FechaPrestamo, fechaLimite);

                if (!libroDisponible)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nEl libro no está disponible en las fechas seleccionadas.");
                    Console.ResetColor();
                    Console.WriteLine("\nPresione cualquier tecla para volver...");
                    Console.ReadKey(true);
                    return;
                }
            }

            // Seleccionar usuario
            var usuariosActivos = Program.Usuarios.Where(u => u.Activo).ToList();
            if (usuariosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay usuarios disponibles.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            string[] nombresUsuarios = usuariosActivos.Select(u => $"{u.Nombre} {u.Apellido}").ToArray();
            int usuarioSeleccionado = Selection_Menu.Print("Seleccionar Usuario", 0, nombresUsuarios);
            prestamo.Usuario = usuariosActivos[usuarioSeleccionado];

            // Establecer fecha límite de devolución (14 días por defecto)
            prestamo.FechaLimiteDevolucion = prestamo.FechaPrestamo.AddDays(14);

            // Para préstamos históricos, permitir establecer si ya fue devuelto
            if (prestamo.FechaPrestamo < fechaActual.Date)
            {
                string[] opcionesEstado = { "Devuelto", "No devuelto (Vencido/Activo)" };
                int estadoSeleccionado = Selection_Menu.Print("¿El libro fue devuelto?", 0, opcionesEstado);

                if (estadoSeleccionado == 0) // Devuelto
                {
                    // Solicitar fecha de devolución real
                    DateTime fechaDevolucion = Validations.Date_input("Ingrese la fecha de devolución (dd/MM/yyyy):");

                    // Validar que la fecha de devolución sea posterior a la de préstamo y no mayor que la fecha actual
                    while (fechaDevolucion < prestamo.FechaPrestamo || fechaDevolucion > fechaActual)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        if (fechaDevolucion < prestamo.FechaPrestamo)
                            Console.WriteLine("\nLa fecha de devolución debe ser posterior a la fecha de préstamo.");
                        else
                            Console.WriteLine("\nLa fecha de devolución no puede ser posterior a la fecha actual.");
                        Console.ResetColor();
                        fechaDevolucion = Validations.Date_input("Ingrese la fecha de devolución (dd/MM/yyyy):");
                    }

                    // Establecer fecha de devolución (el estado se calculará como "Devuelto")
                    prestamo.FechaDevolucionReal = fechaDevolucion;
                }
            }

            // Agregar el préstamo a la lista
            Program.Prestamos.Add(prestamo);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nPréstamo agregado con éxito.");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Menú listar
        public static void Listar()
        {
            string[] opciones = {
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
                opcion = Selection_Menu.Print("Listar Préstamos", 0, opciones) + 1;

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
                        MostrarPrestamos(Program.Prestamos, "Todos los Préstamos");
                        break;
                    case 5:
                        return;
                }
            } while (opcion != 5);
        }

        // Mostrar préstamos
        private static void MostrarPrestamos(List<Prestamo> prestamos, string titulo)
        {
            Console.Clear();

            // Filtrar préstamos activos
            prestamos = prestamos.Where(p => p.Activo).ToList();

            if (prestamos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se encontraron préstamos.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            bool volverAMostrar = true;
            while (volverAMostrar)
            {
                Console.Clear();
                prestamos = Ordenar(prestamos);

                string[] prestamosInfo = prestamos.Select(p =>
                    $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
                ).ToArray();

                // Añadir opción "Volver" al final
                List<string> opcionesConVolver = new List<string>(prestamosInfo);
                opcionesConVolver.Add("Volver");

                int seleccionado = Selection_Menu.Print(titulo, 0, opcionesConVolver.ToArray());

                // Si seleccionó "Volver", salir del bucle
                if (seleccionado == opcionesConVolver.Count - 1)
                {
                    volverAMostrar = false;
                }
                else
                {
                    // Mostrar detalle y esperar a que el usuario presione una tecla
                    MostrarDetallePrestamo(prestamos[seleccionado]);
                }
            }
        }

        // Mostrar detalles del préstamo
        private static void MostrarDetallePrestamo(Prestamo p)
        {
            Console.Clear();
            Console.WriteLine("=== Detalle del Préstamo ===\n");
            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Libro: {p.Libro.Nombre}");
            Console.WriteLine($"Autor: {p.Libro.Autor}");
            Console.WriteLine($"Usuario: {p.Usuario.Nombre} {p.Usuario.Apellido}");
            Console.WriteLine($"Fecha de préstamo: {p.FechaPrestamo:dd/MM/yyyy}");
            Console.WriteLine($"Fecha límite: {p.FechaLimiteDevolucion:dd/MM/yyyy}");
            Console.WriteLine($"Fecha de devolución: {(p.FechaDevolucionReal.HasValue ? p.FechaDevolucionReal.Value.ToString("dd/MM/yyyy") : "No devuelto")}");

            Console.Write("Estado: ");
            switch (p.Estado)
            {
                case EstadoPrestamo.Pendiente:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Pendiente");
                    break;
                case EstadoPrestamo.Activo:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Activo");
                    break;
                case EstadoPrestamo.Devuelto:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Devuelto");
                    break;
                case EstadoPrestamo.Vencido:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Vencido");
                    break;
            }
            Console.ResetColor();

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
            var prestamosFiltrados = Program.Prestamos
                .Where(p => p.Estado == estado && p.Activo)
                .ToList();

            MostrarPrestamos(prestamosFiltrados, $"Préstamos con estado: {estado}");
        }

        // Filtrar por tema
        private static void FiltrarPorTema()
        {
            Console.Clear();
            // Obtener temas únicos de los libros
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
                .Where(p => p.Libro.Temas.Any(t => t.Nombre == tema) && p.Activo)  // Filtrar por tema Y activo
                .ToList();

            MostrarPrestamos(prestamosFiltrados, $"Préstamos de libros con tema: {tema}");
        }

        // Filtrar por usuario
        private static void FiltrarPorUsuario()
        {
            Console.Clear();
            // Crear lista de usuarios con préstamos
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
                .Where(p => p.Usuario.Id == usuario.Id && p.Activo)
                .ToList();

            MostrarPrestamos(prestamosFiltrados, $"Préstamos del usuario: {usuario.Nombre} {usuario.Apellido}");
        }

        // Ordenar por fecha de préstamo (descendente)
        public static List<Prestamo> Ordenar(List<Prestamo> prestamos)
        {
            return prestamos.OrderByDescending(p => p.FechaPrestamo).ToList();
        }

        // Obtener el ID máximo
        public static int MaximoId()
        {
            int max = 0;
            foreach (Prestamo prestamo in Program.Prestamos) if (prestamo.Id > max) max = prestamo.Id;
            return max + 1;
        }

        // Modificar préstamo
        public static void Modificar()
        {
            Console.Clear();

            // Verificar si hay préstamos activos
            var prestamosActivos = Program.Prestamos.Where(p => p.Activo).ToList();
            if (prestamosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen préstamos para modificar.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de préstamos activos para selección
            string[] prestamoInfos = prestamosActivos.Select(p =>
                $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
            ).ToArray();

            int seleccionado = Selection_Menu.Print("Seleccione un préstamo para modificar", 0, prestamoInfos);
            Prestamo prestamoSeleccionado = prestamosActivos[seleccionado];

            string[] modificaciones = { "Modificar Usuario", "Modificar Libro", "Modificar Fecha", "Modificar Fecha Límite",
                "Modificar Fecha Devolución", "Volver" };
            int opcionModificar = Selection_Menu.Print("Seleccione un campo para modificar", 0, modificaciones);
            DateTime fechaActual = DateTime.Now;
            switch (opcionModificar)
            {
                case 0: ModificarUsuarioPrestamo(prestamoSeleccionado); break;
                case 1: ModificarLibroPrestamo(prestamoSeleccionado, fechaActual); break;
                case 2: ModificarFechaPrestamo(prestamoSeleccionado, fechaActual); break;
                case 3: ModificarFechaLimite(prestamoSeleccionado, fechaActual); break;
                case 4: ModificarFechaDevolucion(prestamoSeleccionado, fechaActual); break;
                case 5: break;
            }
        }

        // Modificar Usuario del préstamo 
        private static void ModificarUsuarioPrestamo(Prestamo prestamo)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Usuario del Préstamo ===\n");

            // Obtener usuarios activos
            var usuariosActivos = Program.Usuarios.Where(u => u.Activo).ToList();
            if (usuariosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo hay usuarios disponibles.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de usuarios disponibles
            string[] nombresUsuarios = usuariosActivos.Select(u => $"{u.Nombre} {u.Apellido}").ToArray();
            int usuarioSeleccionado = Selection_Menu.Print("Seleccione el nuevo usuario", 0, nombresUsuarios);

            // Confirmar cambio
            Console.WriteLine($"\n¿Desea cambiar el usuario de {prestamo.Usuario.Nombre} {prestamo.Usuario.Apellido} a {usuariosActivos[usuarioSeleccionado].Nombre} {usuariosActivos[usuarioSeleccionado].Apellido}?");
            string[] opcionesConfirmar = { "Sí", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                prestamo.Usuario = usuariosActivos[usuarioSeleccionado];
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nUsuario modificado con éxito.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Libro del préstamo
        private static void ModificarLibroPrestamo(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Libro del Préstamo ===\n");

            // La modificación del libro depende del estado del préstamo
            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nNo se puede modificar el libro de un préstamo ya devuelto.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            Libro libroOriginal = prestamo.Libro;

            // Para préstamos futuros o actuales, verificar disponibilidad
            if (prestamo.Estado == EstadoPrestamo.Pendiente || prestamo.Estado == EstadoPrestamo.Activo)
            {
                // Seleccionar libro con verificación de disponibilidad
                Libro nuevoLibro;

                if (prestamo.FechaPrestamo <= fechaActual.Date)
                {
                    // Para préstamos actuales o históricos
                    var librosDisponibles = nLibro.VerificarDisponibilidad();
                    if (librosDisponibles.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo hay libros disponibles para préstamo en este momento.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    string[] nombresLibros = librosDisponibles.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                    int libroSeleccionado = Selection_Menu.Print("Seleccione el nuevo libro", 0, nombresLibros);
                    nuevoLibro = librosDisponibles[libroSeleccionado];
                }
                else
                {
                    // Para préstamos futuros
                    nuevoLibro = nLibro.SeleccionarLibro("Seleccione el nuevo libro");

                    if (nuevoLibro == null) return;

                    // Verificar disponibilidad en las fechas
                    bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(nuevoLibro, prestamo.FechaPrestamo, prestamo.FechaLimiteDevolucion);

                    if (!libroDisponible)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nEl libro no está disponible en las fechas del préstamo.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }
                }

                // Confirmar cambio
                Console.WriteLine($"\n¿Desea cambiar el libro de \"{libroOriginal.Nombre}\" a \"{nuevoLibro.Nombre}\"?");
                string[] opcionesConfirmar = { "Sí", "No" };
                int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

                if (confirmar == 0)
                {
                    prestamo.Libro = nuevoLibro;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nLibro modificado con éxito.");
                    Console.ResetColor();
                }
            }
            else if (prestamo.Estado == EstadoPrestamo.Vencido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: El préstamo está vencido. Considere registrar su devolución en lugar de cambiar el libro.");
                Console.ResetColor();

                // A pesar de la advertencia, permitir el cambio si se confirma
                string[] opcionesContinuar = { "Continuar con la modificación", "Cancelar" };
                int continuar = Selection_Menu.Print("¿Desea continuar?", 0, opcionesContinuar);

                if (continuar == 0)
                {
                    Libro nuevoLibro = nLibro.SeleccionarLibro("Seleccione el nuevo libro");
                    if (nuevoLibro != null)
                    {
                        prestamo.Libro = nuevoLibro;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nLibro modificado con éxito.");
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha de Préstamo
        private static void ModificarFechaPrestamo(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha de Préstamo ===\n");

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: Este préstamo ya fue devuelto. Cambiar la fecha de préstamo podría afectar su estado.");
                Console.ResetColor();
            }

            DateTime fechaOriginal = prestamo.FechaPrestamo;

            // Obtener nueva fecha
            DateTime nuevaFecha = Validations.Date_input("Ingrese la nueva fecha de préstamo (dd/MM/yyyy):");

            // Validaciones
            if (nuevaFecha > prestamo.FechaDevolucionReal && prestamo.FechaDevolucionReal.HasValue)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nLa fecha de préstamo no puede ser posterior a la fecha de devolución.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Verificar disponibilidad del libro en la nueva fecha
            bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(
                prestamo.Libro,
                nuevaFecha,
                nuevaFecha.AddDays((prestamo.FechaLimiteDevolucion - prestamo.FechaPrestamo).Days)
            );

            if (!libroDisponible && nuevaFecha != fechaOriginal)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nEl libro no está disponible en la nueva fecha seleccionada.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar cambio
            Console.WriteLine($"\n¿Desea cambiar la fecha de préstamo del {fechaOriginal:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?");
            string[] opcionesConfirmar = { "Sí", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                // Calcular días de diferencia para mantener la duración del préstamo
                int diasDiferencia = (prestamo.FechaLimiteDevolucion - prestamo.FechaPrestamo).Days;

                prestamo.FechaPrestamo = nuevaFecha;
                prestamo.FechaLimiteDevolucion = nuevaFecha.AddDays(diasDiferencia);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nFecha de préstamo modificada con éxito.");
                Console.WriteLine($"La fecha límite de devolución se ajustó a: {prestamo.FechaLimiteDevolucion:dd/MM/yyyy}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha Límite de Devolución
        private static void ModificarFechaLimite(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha Límite de Devolución ===\n");

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: Este préstamo ya fue devuelto. Cambiar la fecha límite no afectará su estado.");
                Console.ResetColor();
            }

            DateTime fechaOriginal = prestamo.FechaLimiteDevolucion;

            // Obtener nueva fecha
            DateTime nuevaFecha = Validations.Date_input("Ingrese la nueva fecha límite (dd/MM/yyyy):");

            // Validaciones
            if (nuevaFecha < prestamo.FechaPrestamo)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nLa fecha límite no puede ser anterior a la fecha de préstamo.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            if (prestamo.FechaDevolucionReal.HasValue && nuevaFecha < prestamo.FechaDevolucionReal.Value)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: La nueva fecha límite es anterior a la fecha de devolución real.");
                Console.ResetColor();
            }

            // Verificar disponibilidad del libro hasta la nueva fecha límite
            bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(
                prestamo.Libro,
                prestamo.FechaPrestamo,
                nuevaFecha
            );

            if (!libroDisponible && nuevaFecha != fechaOriginal)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nEl libro no está disponible hasta la nueva fecha límite seleccionada.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar cambio
            Console.WriteLine($"\n¿Desea cambiar la fecha límite de devolución del {fechaOriginal:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?");
            string[] opcionesConfirmar = { "Sí", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                prestamo.FechaLimiteDevolucion = nuevaFecha;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nFecha límite de devolución modificada con éxito.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha de Devolución Real
        private static void ModificarFechaDevolucion(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha de Devolución Real ===\n");

            string[] opciones = { "Establecer fecha de devolución", "Eliminar fecha de devolución (marcar como no devuelto)", "Cancelar" };
            int opcion = Selection_Menu.Print("Seleccione una opción", 0, opciones);

            switch (opcion)
            {
                case 0: // Establecer fecha
                    DateTime nuevaFecha = Validations.Date_input("Ingrese la fecha de devolución (dd/MM/yyyy):");

                    // Validaciones
                    if (nuevaFecha < prestamo.FechaPrestamo)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nLa fecha de devolución debe ser posterior a la fecha de préstamo.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    if (nuevaFecha > fechaActual)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nLa fecha de devolución no puede ser posterior a la fecha actual.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Confirmar cambio
                    string mensajeConfirmar = prestamo.FechaDevolucionReal.HasValue
                        ? $"¿Desea cambiar la fecha de devolución del {prestamo.FechaDevolucionReal.Value:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?"
                        : $"¿Desea establecer la fecha de devolución al {nuevaFecha:dd/MM/yyyy}?";

                    Console.WriteLine("\n" + mensajeConfirmar);
                    string[] opcionesConfirmar = { "Sí", "No" };
                    int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

                    if (confirmar == 0)
                    {
                        prestamo.FechaDevolucionReal = nuevaFecha;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nFecha de devolución modificada con éxito.");
                        Console.ResetColor();
                    }
                    break;

                case 1: // Eliminar fecha
                    if (!prestamo.FechaDevolucionReal.HasValue)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("\nEl préstamo ya está marcado como no devuelto.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Verificar si hay algún préstamo activo o pendiente del mismo libro
                    bool existePrestamoActivoOMismLibro = Program.Prestamos.Any(p =>
                        p.Id != prestamo.Id &&  // Que no sea el mismo préstamo
                        p.Libro.Id == prestamo.Libro.Id &&  // Que sea el mismo libro
                        (p.Estado == EstadoPrestamo.Activo ||
                         p.Estado == EstadoPrestamo.Pendiente ||
                         p.Estado == EstadoPrestamo.Vencido)  // Que esté activo, pendiente o vencido
                    );

                    if (existePrestamoActivoOMismLibro)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo se puede marcar este préstamo como 'no devuelto'.");
                        Console.WriteLine("El libro ya está actualmente en préstamo o vencido en otra transacción.");
                        Console.WriteLine("Esto generaría una inconsistencia en el sistema.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Confirmar eliminación
                    Console.WriteLine("\n¿Está seguro que desea eliminar la fecha de devolución?");
                    Console.WriteLine("Esto marcará el préstamo como no devuelto (Activo o Vencido según la fecha).");

                    string[] opcionesEliminar = { "Sí", "No" };
                    int confirmarEliminar = Selection_Menu.Print("Confirmar eliminación", 0, opcionesEliminar);

                    if (confirmarEliminar == 0)
                    {
                        prestamo.FechaDevolucionReal = null;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nFecha de devolución eliminada con éxito.");
                        Console.WriteLine("El préstamo ahora está marcado como no devuelto.");
                        Console.ResetColor();
                    }
                    break;

                case 2: // Cancelar
                    return;
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
        // Eliminar préstamo (eliminación lógica)
        public static void Eliminar()
        {
            Console.Clear();

            // Verificar si hay préstamos activos en el sistema
            var prestamosActivos = Program.Prestamos.Where(p => p.Activo).ToList();
            if (prestamosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen préstamos para eliminar.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de préstamos activos para selección
            string[] prestamoInfos = prestamosActivos.Select(p =>
                $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
            ).ToArray();

            int seleccionado = Selection_Menu.Print("Seleccione un préstamo para eliminar", 0, prestamoInfos);
            Prestamo prestamoSeleccionado = prestamosActivos[seleccionado];

            // Validar que solo se puedan eliminar préstamos devueltos
            if (prestamoSeleccionado.Estado != EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se puede eliminar un préstamo que no ha sido devuelto.");
                Console.WriteLine("Solo se pueden eliminar préstamos con estado 'Devuelto'.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar eliminación
            Console.WriteLine($"\n¿Está seguro que desea eliminar el préstamo ID: {prestamoSeleccionado.Id}?");
            Console.WriteLine($"Libro: {prestamoSeleccionado.Libro.Nombre}");
            Console.WriteLine($"Usuario: {prestamoSeleccionado.Usuario.Nombre} {prestamoSeleccionado.Usuario.Apellido}");
            Console.WriteLine($"Fecha: {prestamoSeleccionado.FechaPrestamo:dd/MM/yyyy}");

            string[] opcionesConfirmar = { "Sí", "No" };
            int confirmar = Selection_Menu.Print("Confirmar eliminación", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                // En lugar de eliminar el préstamo de la lista, marcarlo como inactivo
                prestamoSeleccionado.Activo = false;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nPréstamo eliminado con éxito.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}