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
            usuario.Email = Validations.Characters_input();          

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
            string[] opciones = new string[] { "Cambiar nombre", "Cambiar Apellido", "Cambiar Email", "Salir" };
            int opcion = Selection_Menu.Print(usuario.Nombre + " " + usuario.Apellido, 0, opciones);
            switch (opcion)
            {
                case 0: Console.Write("Ingrese Nuevo Nombre: "); usuario.Nombre = Validations.Letters_only_input(); Modificar(usuario); break;
                case 1: Console.Write("Ingrese Nuevo Apellido: "); usuario.Apellido = Validations.Letters_only_input(); Modificar(usuario); break;
                case 2: Console.Write("Ingrese Nuevo Email: "); usuario.Email = Validations.Characters_input(); Modificar(usuario); break;
                case 3: Menu(); break;
                default: Modificar(usuario); break;
            }
        }


        public static void Eliminar(Usuario usuario)
        {

            bool TienePrestamosActivos = usuario.Prestamos.Any(p => p.Estado == EstadoPrestamo.Activo || p.Estado == EstadoPrestamo.Vencido);

            if (TienePrestamosActivos)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo se puede eliminar el usuario porque tiene prestamos activos o vencidos");
                Console.ResetColor();
            }
            else
            {
                usuario.Activo = false; //Desactivo el usuario aca
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nUsuario eliminado con éxito.");
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

            var usuariosActivos = Program.Usuarios.Where(u => u.Activo).ToList();
            string[] nombres = usuariosActivos.Select(u => u.Nombre + " " + u.Apellido).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Volver" };
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

                    Modificar(usuariosActivos[Selection_Menu.Print("Lista de Usuarios", 0, nombres)]); Menu(); break;
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

                    Eliminar(usuariosActivos[Selection_Menu.Print("Lista de Usuarios", 0, nombres)]); Menu(); break;
                case 3: break;
                default: Menu(); break;
            }
        }
    }
}
 
