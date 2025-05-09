using Mi_libreria;
using TP_Biblioteca.Modelos;

namespace TP_Biblioteca.Controladores
{
    class nUsuario
    {

        public static void Agregar()
        {
            Console.Clear();
            bool existe = false;
            Usuario usuario = new Usuario();
            usuario.Id = MaximoId();
            Console.Write("Coloque el nombre del usuario: ");
            usuario.Nombre = Validations.Letters_only_input();
            Console.Write("Coloque el apellido del usuario: ");
            usuario.Apellido = Validations.Letters_only_input();
            Console.Write("Coloque el Email del usuario: ");
            usuario.Email = Validations.Alphanumeric_input();

            foreach (Usuario u in Program.Usuarios)
            {
                if (u.Id == usuario.Id && u.Nombre == usuario.Nombre && u.Apellido == usuario.Apellido && u.Email == usuario.Email)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nEl usuario que desea agregar ya existe");
                    Console.ResetColor();
                    existe = true;
                }
            }
            if (existe == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nEl usuario se agregó con éxito");
                Console.ResetColor();
                Program.Usuarios.Add(usuario);
            }
            Console.ReadKey(true);
        }

        public static void Modificar(Usuario usuario)
        {
            string[] opciones = { "Cambiar nombre", "Cambiar Apellido", "Cambiar Email", "Salir" };
            int opcion = Selection_Menu.Print(usuario.Nombre + " " + usuario.Apellido, 0, opciones);
            switch (opcion)
            {
                case 0: Console.Write("Ingrese Nuevo Nombre: "); usuario.Nombre = Validations.Letters_only_input(); Modificar(usuario); break;
                case 1: Console.Write("Ingrese Nuevo Apellido: "); usuario.Apellido = Validations.Letters_only_input(); Modificar(usuario); break;
                case 2: Console.Write("Ingrese Nuevo Email: "); usuario.Email = Validations.Alphanumeric_input(); Modificar(usuario); break;
                case 3: break;
                default: Modificar(usuario); break;
            }
        }

        public static void Eliminar(Usuario usuario)
        {
            // Verificar préstamos en la lista global de préstamos
            bool tienePrestamosActivos = Program.Prestamos.Any(p =>
                p.Usuario.Id == usuario.Id &&
                (p.Estado == EstadoPrestamo.Activo || p.Estado == EstadoPrestamo.Vencido || p.Estado == EstadoPrestamo.Pendiente)
            );

            if (tienePrestamosActivos)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se puede eliminar el usuario porque tiene préstamos activos, pendientes o vencidos");
                Console.ResetColor();
            }
            else
            {
                usuario.Activo = false; // Desactivar en lugar de eliminar
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nUsuario desactivado con éxito.");
                Console.ResetColor();
            }

            Console.ReadKey(true);
        }

        public static int MaximoId()
        {
            int max = 0;
            foreach (Usuario usuario in Program.Usuarios) if (usuario.Id > max) max = usuario.Id;
            return max + 1;
        }
   
        public static void Menu()
        {
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Usuarios", 0, opciones);
            switch (opcion)
            {
                case 0: Console.Clear(); Agregar(); Menu(); break;
                case 1:
                    Console.Clear();
                    if (Program.Usuarios.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    var usuariosActivosModificar = Program.Usuarios.Where(u => u.Activo).ToList();
                    string[] nombresModificar = usuariosActivosModificar.Select(u => $"{u.Id} | {u.Nombre} {u.Apellido}").ToArray();
                    Modificar(usuariosActivosModificar[Selection_Menu.Print("Lista de Usuarios", 0, nombresModificar)]); Menu(); break;
                case 2:
                    Console.Clear();
                    if (Program.Usuarios.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    var usuariosActivosEliminar = Program.Usuarios.Where(u => u.Activo).ToList();
                    string[] nombresEliminar = usuariosActivosEliminar.Select(u => $"{u.Id} | {u.Nombre} {u.Apellido}").ToArray();
                    Eliminar(usuariosActivosEliminar[Selection_Menu.Print("Lista de Usuarios", 0, nombresEliminar)]); Menu(); break;
                case 3: Console.Clear(); Listar(); Menu(); break;
                case 4: break; // Volver
                default: Menu(); break;
            }
        }
        public static void Listar()
        {
            Console.Clear();

            // Verificar que existan usuarios
            if (Program.Usuarios.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen usuarios registrados");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }

            // Filtrar usuarios activos
            var usuariosActivos = Program.Usuarios.Where(u => u.Activo).ToList();
            if (usuariosActivos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen usuarios activos");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }

            // Mostrar lista de usuarios para selección
            string[] nombresUsuarios = usuariosActivos.Select(u => $"{u.Id} | {u.Nombre} {u.Apellido}").ToArray();

            // Agregar opción para volver
            List<string> opcionesConVolver = new List<string>(nombresUsuarios);
            opcionesConVolver.Add("Volver");

            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                int seleccionado = Selection_Menu.Print("Lista de Usuarios", 0, opcionesConVolver.ToArray());

                // Verificar si seleccionó "Volver"
                if (seleccionado == opcionesConVolver.Count - 1)
                {
                    continuar = false;
                }
                else
                {
                    // Mostrar los detalles del usuario seleccionado
                    MostrarDetalleUsuario(usuariosActivos[seleccionado]);
                }
            }
        }

        // Mostrar los detalles de un usuario específico
        private static void MostrarDetalleUsuario(Usuario usuario)
        {
            Console.Clear();
            Console.WriteLine("=== Detalle del Usuario ===\n");
            Console.WriteLine($"ID: {usuario.Id}");
            Console.WriteLine($"Nombre: {usuario.Nombre} {usuario.Apellido}");
            Console.WriteLine($"Email: {usuario.Email}");

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey(true);
        }
    }
}
 
