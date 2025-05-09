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
            string[] nombres = Program.Prestamos
                .Where(p => p.Activo)  // Filtrar solo pr�stamos activos
                .Select(p => $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}")
                .ToArray();
            string[] opciones = { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            int opcion;

            do
            {
                Console.Clear();
                opcion = Selection_Menu.Print("Pr�stamos", 0, opciones);

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

        // Agregar pr�stamo
        public static void Agregar()
        {
            Console.Clear();

            // Crear un nuevo pr�stamo
            Prestamo prestamo = new Prestamo();

            // Asignar ID
            prestamo.Id = MaximoId();

            // Preguntar primero por la fecha del pr�stamo
            string[] opcionFecha = { "Fecha actual", "Fecha personalizada" };
            int fechaSeleccionada = Selection_Menu.Print("Seleccione la fecha del pr�stamo", 0, opcionFecha);

            DateTime fechaActual = DateTime.Now;

            if (fechaSeleccionada == 0) // Fecha actual
            {
                prestamo.FechaPrestamo = fechaActual;

                // Para pr�stamos actuales, mostrar solo libros actualmente disponibles
                var librosDisponibles = nLibro.VerificarDisponibilidad();
                if (librosDisponibles.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nNo hay libros disponibles para pr�stamo en este momento.");
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
                prestamo.FechaPrestamo = Validations.Date_input("Ingrese la fecha de pr�stamo (dd/MM/yyyy):");

                // Seleccionar libro de todos los libros no eliminados
                prestamo.Libro = nLibro.SeleccionarLibro("Seleccione un libro para el pr�stamo");

                // Si no se seleccion� ning�n libro, cancelar
                if (prestamo.Libro == null) return;

                // Calcular fecha l�mite de devoluci�n (14 d�as por defecto)
                DateTime fechaLimite = prestamo.FechaPrestamo.AddDays(14);

                // Verificar si el libro est� disponible en esas fechas
                bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(prestamo.Libro, prestamo.FechaPrestamo, fechaLimite);

                if (!libroDisponible)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nEl libro no est� disponible en las fechas seleccionadas.");
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

            // Establecer fecha l�mite de devoluci�n (14 d�as por defecto)
            prestamo.FechaLimiteDevolucion = prestamo.FechaPrestamo.AddDays(14);

            // Para pr�stamos hist�ricos, permitir establecer si ya fue devuelto
            if (prestamo.FechaPrestamo < fechaActual.Date)
            {
                string[] opcionesEstado = { "Devuelto", "No devuelto (Vencido/Activo)" };
                int estadoSeleccionado = Selection_Menu.Print("�El libro fue devuelto?", 0, opcionesEstado);

                if (estadoSeleccionado == 0) // Devuelto
                {
                    // Solicitar fecha de devoluci�n real
                    DateTime fechaDevolucion = Validations.Date_input("Ingrese la fecha de devoluci�n (dd/MM/yyyy):");

                    // Validar que la fecha de devoluci�n sea posterior a la de pr�stamo y no mayor que la fecha actual
                    while (fechaDevolucion < prestamo.FechaPrestamo || fechaDevolucion > fechaActual)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        if (fechaDevolucion < prestamo.FechaPrestamo)
                            Console.WriteLine("\nLa fecha de devoluci�n debe ser posterior a la fecha de pr�stamo.");
                        else
                            Console.WriteLine("\nLa fecha de devoluci�n no puede ser posterior a la fecha actual.");
                        Console.ResetColor();
                        fechaDevolucion = Validations.Date_input("Ingrese la fecha de devoluci�n (dd/MM/yyyy):");
                    }

                    // Establecer fecha de devoluci�n (el estado se calcular� como "Devuelto")
                    prestamo.FechaDevolucionReal = fechaDevolucion;
                }
            }

            // Agregar el pr�stamo a la lista
            Program.Prestamos.Add(prestamo);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nPr�stamo agregado con �xito.");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Men� listar
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

            // Filtrar pr�stamos activos
            prestamos = prestamos.Where(p => p.Activo).ToList();

            if (prestamos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se encontraron pr�stamos.");
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

                // A�adir opci�n "Volver" al final
                List<string> opcionesConVolver = new List<string>(prestamosInfo);
                opcionesConVolver.Add("Volver");

                int seleccionado = Selection_Menu.Print(titulo, 0, opcionesConVolver.ToArray());

                // Si seleccion� "Volver", salir del bucle
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

        // Mostrar detalles del pr�stamo
        private static void MostrarDetallePrestamo(Prestamo p)
        {
            Console.Clear();
            Console.WriteLine("=== Detalle del Pr�stamo ===\n");
            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Libro: {p.Libro.Nombre}");
            Console.WriteLine($"Autor: {p.Libro.Autor}");
            Console.WriteLine($"Usuario: {p.Usuario.Nombre} {p.Usuario.Apellido}");
            Console.WriteLine($"Fecha de pr�stamo: {p.FechaPrestamo:dd/MM/yyyy}");
            Console.WriteLine($"Fecha l�mite: {p.FechaLimiteDevolucion:dd/MM/yyyy}");
            Console.WriteLine($"Fecha de devoluci�n: {(p.FechaDevolucionReal.HasValue ? p.FechaDevolucionReal.Value.ToString("dd/MM/yyyy") : "No devuelto")}");

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
                .Where(p => p.Libro.Temas.Any(t => t.Nombre == tema) && p.Activo)  // Filtrar por tema Y activo
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
                .Where(p => p.Usuario.Id == usuario.Id && p.Activo)
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

        // Modificar pr�stamo
        public static void Modificar()
        {
            Console.Clear();

            // Verificar si hay pr�stamos activos
            var prestamosActivos = Program.Prestamos.Where(p => p.Activo).ToList();
            if (prestamosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen pr�stamos para modificar.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de pr�stamos activos para selecci�n
            string[] prestamoInfos = prestamosActivos.Select(p =>
                $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
            ).ToArray();

            int seleccionado = Selection_Menu.Print("Seleccione un pr�stamo para modificar", 0, prestamoInfos);
            Prestamo prestamoSeleccionado = prestamosActivos[seleccionado];

            string[] modificaciones = { "Modificar Usuario", "Modificar Libro", "Modificar Fecha", "Modificar Fecha L�mite",
                "Modificar Fecha Devoluci�n", "Volver" };
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

        // Modificar Usuario del pr�stamo 
        private static void ModificarUsuarioPrestamo(Prestamo prestamo)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Usuario del Pr�stamo ===\n");

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
            Console.WriteLine($"\n�Desea cambiar el usuario de {prestamo.Usuario.Nombre} {prestamo.Usuario.Apellido} a {usuariosActivos[usuarioSeleccionado].Nombre} {usuariosActivos[usuarioSeleccionado].Apellido}?");
            string[] opcionesConfirmar = { "S�", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                prestamo.Usuario = usuariosActivos[usuarioSeleccionado];
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nUsuario modificado con �xito.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Libro del pr�stamo
        private static void ModificarLibroPrestamo(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Libro del Pr�stamo ===\n");

            // La modificaci�n del libro depende del estado del pr�stamo
            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nNo se puede modificar el libro de un pr�stamo ya devuelto.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            Libro libroOriginal = prestamo.Libro;

            // Para pr�stamos futuros o actuales, verificar disponibilidad
            if (prestamo.Estado == EstadoPrestamo.Pendiente || prestamo.Estado == EstadoPrestamo.Activo)
            {
                // Seleccionar libro con verificaci�n de disponibilidad
                Libro nuevoLibro;

                if (prestamo.FechaPrestamo <= fechaActual.Date)
                {
                    // Para pr�stamos actuales o hist�ricos
                    var librosDisponibles = nLibro.VerificarDisponibilidad();
                    if (librosDisponibles.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo hay libros disponibles para pr�stamo en este momento.");
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
                    // Para pr�stamos futuros
                    nuevoLibro = nLibro.SeleccionarLibro("Seleccione el nuevo libro");

                    if (nuevoLibro == null) return;

                    // Verificar disponibilidad en las fechas
                    bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(nuevoLibro, prestamo.FechaPrestamo, prestamo.FechaLimiteDevolucion);

                    if (!libroDisponible)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nEl libro no est� disponible en las fechas del pr�stamo.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }
                }

                // Confirmar cambio
                Console.WriteLine($"\n�Desea cambiar el libro de \"{libroOriginal.Nombre}\" a \"{nuevoLibro.Nombre}\"?");
                string[] opcionesConfirmar = { "S�", "No" };
                int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

                if (confirmar == 0)
                {
                    prestamo.Libro = nuevoLibro;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\nLibro modificado con �xito.");
                    Console.ResetColor();
                }
            }
            else if (prestamo.Estado == EstadoPrestamo.Vencido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: El pr�stamo est� vencido. Considere registrar su devoluci�n en lugar de cambiar el libro.");
                Console.ResetColor();

                // A pesar de la advertencia, permitir el cambio si se confirma
                string[] opcionesContinuar = { "Continuar con la modificaci�n", "Cancelar" };
                int continuar = Selection_Menu.Print("�Desea continuar?", 0, opcionesContinuar);

                if (continuar == 0)
                {
                    Libro nuevoLibro = nLibro.SeleccionarLibro("Seleccione el nuevo libro");
                    if (nuevoLibro != null)
                    {
                        prestamo.Libro = nuevoLibro;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nLibro modificado con �xito.");
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha de Pr�stamo
        private static void ModificarFechaPrestamo(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha de Pr�stamo ===\n");

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: Este pr�stamo ya fue devuelto. Cambiar la fecha de pr�stamo podr�a afectar su estado.");
                Console.ResetColor();
            }

            DateTime fechaOriginal = prestamo.FechaPrestamo;

            // Obtener nueva fecha
            DateTime nuevaFecha = Validations.Date_input("Ingrese la nueva fecha de pr�stamo (dd/MM/yyyy):");

            // Validaciones
            if (nuevaFecha > prestamo.FechaDevolucionReal && prestamo.FechaDevolucionReal.HasValue)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nLa fecha de pr�stamo no puede ser posterior a la fecha de devoluci�n.");
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
                Console.WriteLine("\nEl libro no est� disponible en la nueva fecha seleccionada.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar cambio
            Console.WriteLine($"\n�Desea cambiar la fecha de pr�stamo del {fechaOriginal:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?");
            string[] opcionesConfirmar = { "S�", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                // Calcular d�as de diferencia para mantener la duraci�n del pr�stamo
                int diasDiferencia = (prestamo.FechaLimiteDevolucion - prestamo.FechaPrestamo).Days;

                prestamo.FechaPrestamo = nuevaFecha;
                prestamo.FechaLimiteDevolucion = nuevaFecha.AddDays(diasDiferencia);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nFecha de pr�stamo modificada con �xito.");
                Console.WriteLine($"La fecha l�mite de devoluci�n se ajust� a: {prestamo.FechaLimiteDevolucion:dd/MM/yyyy}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha L�mite de Devoluci�n
        private static void ModificarFechaLimite(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha L�mite de Devoluci�n ===\n");

            if (prestamo.Estado == EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: Este pr�stamo ya fue devuelto. Cambiar la fecha l�mite no afectar� su estado.");
                Console.ResetColor();
            }

            DateTime fechaOriginal = prestamo.FechaLimiteDevolucion;

            // Obtener nueva fecha
            DateTime nuevaFecha = Validations.Date_input("Ingrese la nueva fecha l�mite (dd/MM/yyyy):");

            // Validaciones
            if (nuevaFecha < prestamo.FechaPrestamo)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nLa fecha l�mite no puede ser anterior a la fecha de pr�stamo.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            if (prestamo.FechaDevolucionReal.HasValue && nuevaFecha < prestamo.FechaDevolucionReal.Value)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nAdvertencia: La nueva fecha l�mite es anterior a la fecha de devoluci�n real.");
                Console.ResetColor();
            }

            // Verificar disponibilidad del libro hasta la nueva fecha l�mite
            bool libroDisponible = nLibro.VerificarDisponibilidadEnFecha(
                prestamo.Libro,
                prestamo.FechaPrestamo,
                nuevaFecha
            );

            if (!libroDisponible && nuevaFecha != fechaOriginal)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nEl libro no est� disponible hasta la nueva fecha l�mite seleccionada.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar cambio
            Console.WriteLine($"\n�Desea cambiar la fecha l�mite de devoluci�n del {fechaOriginal:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?");
            string[] opcionesConfirmar = { "S�", "No" };
            int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                prestamo.FechaLimiteDevolucion = nuevaFecha;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nFecha l�mite de devoluci�n modificada con �xito.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }

        // Modificar Fecha de Devoluci�n Real
        private static void ModificarFechaDevolucion(Prestamo prestamo, DateTime fechaActual)
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Fecha de Devoluci�n Real ===\n");

            string[] opciones = { "Establecer fecha de devoluci�n", "Eliminar fecha de devoluci�n (marcar como no devuelto)", "Cancelar" };
            int opcion = Selection_Menu.Print("Seleccione una opci�n", 0, opciones);

            switch (opcion)
            {
                case 0: // Establecer fecha
                    DateTime nuevaFecha = Validations.Date_input("Ingrese la fecha de devoluci�n (dd/MM/yyyy):");

                    // Validaciones
                    if (nuevaFecha < prestamo.FechaPrestamo)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nLa fecha de devoluci�n debe ser posterior a la fecha de pr�stamo.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    if (nuevaFecha > fechaActual)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nLa fecha de devoluci�n no puede ser posterior a la fecha actual.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Confirmar cambio
                    string mensajeConfirmar = prestamo.FechaDevolucionReal.HasValue
                        ? $"�Desea cambiar la fecha de devoluci�n del {prestamo.FechaDevolucionReal.Value:dd/MM/yyyy} al {nuevaFecha:dd/MM/yyyy}?"
                        : $"�Desea establecer la fecha de devoluci�n al {nuevaFecha:dd/MM/yyyy}?";

                    Console.WriteLine("\n" + mensajeConfirmar);
                    string[] opcionesConfirmar = { "S�", "No" };
                    int confirmar = Selection_Menu.Print("Confirmar cambio", 0, opcionesConfirmar);

                    if (confirmar == 0)
                    {
                        prestamo.FechaDevolucionReal = nuevaFecha;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nFecha de devoluci�n modificada con �xito.");
                        Console.ResetColor();
                    }
                    break;

                case 1: // Eliminar fecha
                    if (!prestamo.FechaDevolucionReal.HasValue)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("\nEl pr�stamo ya est� marcado como no devuelto.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Verificar si hay alg�n pr�stamo activo o pendiente del mismo libro
                    bool existePrestamoActivoOMismLibro = Program.Prestamos.Any(p =>
                        p.Id != prestamo.Id &&  // Que no sea el mismo pr�stamo
                        p.Libro.Id == prestamo.Libro.Id &&  // Que sea el mismo libro
                        (p.Estado == EstadoPrestamo.Activo ||
                         p.Estado == EstadoPrestamo.Pendiente ||
                         p.Estado == EstadoPrestamo.Vencido)  // Que est� activo, pendiente o vencido
                    );

                    if (existePrestamoActivoOMismLibro)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo se puede marcar este pr�stamo como 'no devuelto'.");
                        Console.WriteLine("El libro ya est� actualmente en pr�stamo o vencido en otra transacci�n.");
                        Console.WriteLine("Esto generar�a una inconsistencia en el sistema.");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para volver...");
                        Console.ReadKey(true);
                        return;
                    }

                    // Confirmar eliminaci�n
                    Console.WriteLine("\n�Est� seguro que desea eliminar la fecha de devoluci�n?");
                    Console.WriteLine("Esto marcar� el pr�stamo como no devuelto (Activo o Vencido seg�n la fecha).");

                    string[] opcionesEliminar = { "S�", "No" };
                    int confirmarEliminar = Selection_Menu.Print("Confirmar eliminaci�n", 0, opcionesEliminar);

                    if (confirmarEliminar == 0)
                    {
                        prestamo.FechaDevolucionReal = null;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nFecha de devoluci�n eliminada con �xito.");
                        Console.WriteLine("El pr�stamo ahora est� marcado como no devuelto.");
                        Console.ResetColor();
                    }
                    break;

                case 2: // Cancelar
                    return;
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
        // Eliminar pr�stamo (eliminaci�n l�gica)
        public static void Eliminar()
        {
            Console.Clear();

            // Verificar si hay pr�stamos activos en el sistema
            var prestamosActivos = Program.Prestamos.Where(p => p.Activo).ToList();
            if (prestamosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen pr�stamos para eliminar.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de pr�stamos activos para selecci�n
            string[] prestamoInfos = prestamosActivos.Select(p =>
                $"ID: {p.Id} | {p.FechaPrestamo:dd/MM/yyyy} | {p.Libro.Nombre} | {p.Usuario.Nombre} {p.Usuario.Apellido} | {p.Estado}"
            ).ToArray();

            int seleccionado = Selection_Menu.Print("Seleccione un pr�stamo para eliminar", 0, prestamoInfos);
            Prestamo prestamoSeleccionado = prestamosActivos[seleccionado];

            // Validar que solo se puedan eliminar pr�stamos devueltos
            if (prestamoSeleccionado.Estado != EstadoPrestamo.Devuelto)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se puede eliminar un pr�stamo que no ha sido devuelto.");
                Console.WriteLine("Solo se pueden eliminar pr�stamos con estado 'Devuelto'.");
                Console.ResetColor();
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey(true);
                return;
            }

            // Confirmar eliminaci�n
            Console.WriteLine($"\n�Est� seguro que desea eliminar el pr�stamo ID: {prestamoSeleccionado.Id}?");
            Console.WriteLine($"Libro: {prestamoSeleccionado.Libro.Nombre}");
            Console.WriteLine($"Usuario: {prestamoSeleccionado.Usuario.Nombre} {prestamoSeleccionado.Usuario.Apellido}");
            Console.WriteLine($"Fecha: {prestamoSeleccionado.FechaPrestamo:dd/MM/yyyy}");

            string[] opcionesConfirmar = { "S�", "No" };
            int confirmar = Selection_Menu.Print("Confirmar eliminaci�n", 0, opcionesConfirmar);

            if (confirmar == 0)
            {
                // En lugar de eliminar el pr�stamo de la lista, marcarlo como inactivo
                prestamoSeleccionado.Activo = false;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nPr�stamo eliminado con �xito.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nOperaci�n cancelada.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}