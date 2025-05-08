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
    internal class nTema
    {
        public static void Agregar()
        {
            Console.Clear();
            bool existe = false;
            Tema tema = new Tema();
            List<Libro> libros_del_tema = new List<Libro>();
            tema.Id = MaximoId();
            Console.Write("Coloque el nombre: ");
            tema.Nombre = Validations.Letters_only_input();

            foreach (Tema t in Program.Temas)
            {
                if (t.Id == tema.Id && t.Nombre == tema.Nombre)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nEl tema que desea agregar ya existe");
                    Console.ResetColor();
                    existe = true;
                }
            }
            if (existe == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\nEl tema se agregó con éxito");
                Console.ResetColor();
                Program.Temas.Add(tema);
            }
            Console.ReadKey(true);
        }

        public static void Ordenar()
        {
            Program.Temas = Program.Temas.OrderBy(t => t.Nombre).ToList();
        }

        public static void Listar()
        {
            Console.Clear();
            if (Program.Temas.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }
            string[] nombres = Program.Temas.Select(t => t.Nombre).ToArray();
            Tema tema_seleccionado = Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombres)];
            Console.Clear();
            Console.WriteLine($"Nombre del Tema: {tema_seleccionado.Nombre}");
            Console.WriteLine("\nPresione cualquier tecla para continuar");
            Console.ReadKey(true);

            string[] opciones = { "SI", "NO" };
            int opcion = Selection_Menu.Print("Desea ver los libros asignados a este Tema?", 0, opciones);
            switch (opcion)
            {
                case 0: nLibro.Ordenar(); nLibro.Listar(); break;
                case 1: Console.WriteLine("\nPresione cualquier tecla para volver"); Console.ReadKey(true); break;
                default: Listar(); break;
            }
        }

        public static void Modificar(Tema tema)
        {
            string[] opciones = new string[] { "Cambiar nombre", "Salir" };
            int opcion = Selection_Menu.Print(tema.Nombre, 1, opciones);
            switch (opcion)
            {
                case 0: Console.Write("Ingrese Nuevo Nombre: "); tema.Nombre = Validations.Letters_only_input(); Modificar(tema); break;
                case 1: break;
                default: Modificar(tema); break;
            }
        }

        public static void Eliminar(Tema tema)
        {
            if (tema.Libros.Count == 0)
            {
                //Valida si existen libros asociados al tema
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Existen libros asociados a el tema que desea eliminar\nNo puede eliminar el tema");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }
            Program.Temas.Remove(tema);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nTema borrado con éxito");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static void AgregarLibro(Tema tema)
        {
            string[] nombres = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
            if (Program.Libros.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("Libros");
                foreach (string nombre in nombres) Console.WriteLine(nombre);
            }

            string[] opciones = new string[] { "Seleccionar Libro", "Agregar Libro Nuevo", "Volver"};
            int opcion = Selection_Menu.Print("Agregar Libro", 0, opciones);
            switch (opcion)
            {
                case 0: Console.Clear(); Ordenar(); nLibro.Ordenar();
                    Libro libro_seleccionado = Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)];
                    if (tema.Libros.Contains(libro_seleccionado))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nEl libro que desea agregar al Tema ya se encuentra dentro del mismo");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    else
                    {
                        tema.Libros.Add(libro_seleccionado);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\nEl libro se asignó con éxito al Tema seleccionado");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                    break;
                case 1: Console.Clear(); nLibro.Agregar(); AgregarLibro(tema); break;
                case 2: Menu(); break;
                default: AgregarLibro(tema); break;
            }
        }

        public static int MaximoId()
        {
            int max = 0;
            foreach (Tema tema in Program.Temas) if (tema.Id > max) max = tema.Id;
            return max + 1;
        }

        public static void Menu()
        {
            string[] nombres = Program.Temas.Select(l => l.Nombre).ToArray();
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Agregar Libro", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Temas", 0, opciones) + 1;
            switch (opcion)
            {
                case 1: Console.Clear(); Ordenar(); Agregar(); Menu(); break;
                case 2:
                    Console.Clear(); Ordenar();
                    if (Program.Temas.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    Modificar(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombres)]); Menu(); break;
                case 3:
                    Console.Clear(); Ordenar();
                    if (Program.Temas.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    Eliminar(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombres)]); Menu(); break;
                case 4: Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 5: Console.Clear(); Ordenar();
                    if (Program.Temas.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    AgregarLibro(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombres)]); Menu(); break;
                case 6: break;
                default: Menu(); break;
            }
        }
    }
}
