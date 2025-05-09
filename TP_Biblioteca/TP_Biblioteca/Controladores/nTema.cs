﻿using System;
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
            bool existe = false; // Booleano para verificar la existencia del tema que se desea agregar
            bool agregarLibro = true; // Booleano para el while si se desea agregar un libro a la lista de libros del tema
            List<Libro> librosDelTema = new List<Libro>(); // Cada tema tiene una lista de libros
            Tema tema = new Tema();
            tema.Id = MaximoId();
            Console.Write("Coloque el nombre: ");
            tema.Nombre = Validations.Alphanumeric_input();
            tema.Libros = librosDelTema;

            // Si se desea agregar un libro a la lista de libros de un tema
            while (agregarLibro)
            {
                string[] opciones = { "SI", "NO" };
                int opcion = Selection_Menu.Print("¿Desea asignar algún libro a este tema?", 0, opciones);
                switch (opcion)
                {
                    case 0: Console.Clear(); nLibro.Ordenar(); bool flag_libroPorAgregar = false;
                        if (Program.Libros.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nNo existen datos");
                            Console.ResetColor();
                            Console.ReadKey(true);
                            Menu();
                            break;
                        }
                        string[] nombres = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                        Libro libroPorAgregar = (Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)]);
                        foreach (Libro l in tema.Libros)
                        {
                            if (libroPorAgregar.Id == l.Id && libroPorAgregar.Nombre == l.Nombre && 
                                libroPorAgregar.Autor == l.Autor && libroPorAgregar.Prologo == l.Prologo)
                            {
                                flag_libroPorAgregar = true;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"\nEl libro {libroPorAgregar.Nombre} ya contiene este tema");
                                Console.ResetColor();
                            }
                        }
                        if (!flag_libroPorAgregar)
                        {
                            tema.Libros.Add(libroPorAgregar);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\nEl libro se asignó con éxito");
                            Console.ResetColor();
                        }
                        Console.ReadKey(true);
                        agregarLibro = true;  break;
                    case 1: agregarLibro = false;  break;
                    default: agregarLibro = true; break;
                }    
            }

            // Validando que el Tema que se desea agregar no exista ya en la lista de temas
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
            Program.Temas = Program.Temas.OrderByDescending(t => t.Nombre).ToList();
        }

        public static void OrdenarLibros(Tema tema)
        {
            tema.Libros = tema.Libros.OrderBy(l => l.Nombre).ToList();
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

            //No muestra los temas porque no están cargados desde Program.cs
            string[] opciones = { "SI", "NO" };
            int opcion = Selection_Menu.Print("Desea ver los libros asignados a este Tema?", 0, opciones);
            switch (opcion)
            {
                case 0:
                    foreach (Libro l in tema_seleccionado.Libros)
                    {
                        Console.WriteLine(l.Nombre + " By " + l.Autor);
                    }
                    Console.ReadKey(true); break;
                case 1: Console.WriteLine("\nPresione cualquier tecla para volver"); Console.ReadKey(true); break;
                default: Listar(); break;
            }
        }

        public static void Modificar(Tema tema)
        {
            string[] opciones = { "Cambiar nombre", "Salir" };
            int opcion = Selection_Menu.Print(tema.Nombre, 0, opciones);
            switch (opcion)
            {
                case 0: Console.Write("Ingrese Nuevo Nombre: "); tema.Nombre = Validations.Alphanumeric_input(); Modificar(tema); break;
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
                Console.WriteLine("Existen libros asociados al tema que desea eliminar\nNo puede eliminar el tema");
                Console.ResetColor();
                Console.ReadKey(true);
                return;
            }

            //Hay que validar algo?
            foreach (Libro l in tema.Libros)
            {
                l.Temas.Remove(tema);
            }

            Program.Temas.Remove(tema);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nTema borrado con éxito");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static void AgregarLibro(Tema tema)
        {
            Ordenar();
            OrdenarLibros(tema);
            if (Program.Libros.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nNo existen datos");
                Console.ResetColor();
                Console.ReadKey(true);
            }
            /*
            else
            {
                Console.WriteLine("Libros");
                foreach (string nombre in nombres) Console.WriteLine(nombre);
            }
            */

            string[] opciones = { "Seleccionar Libro", "Agregar Libro Nuevo", "Volver" };
            int opcion = Selection_Menu.Print("Agregar Libro", 0, opciones);
            switch (opcion)
            {
                case 0:
                    Console.Clear(); Ordenar(); nLibro.Ordenar(); bool flag_libroAgregar = false;
                    if (Program.Libros.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        AgregarLibro(tema);
                        break;
                    }
                    string[] nombres = Program.Libros.Select(l => l.Nombre + " - By " + l.Autor).ToArray();
                    Libro libro_seleccionado = Program.Libros[Selection_Menu.Print("Lista de Libros", 0, nombres)];
                    foreach (Libro l in tema.Libros)
                    {
                        if (libro_seleccionado.Id == l.Id && libro_seleccionado.Nombre == l.Nombre &&
                            libro_seleccionado.Autor == l.Autor && libro_seleccionado.Prologo == l.Prologo)
                        {
                            flag_libroAgregar = true;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"\nEl libro {libro_seleccionado.Nombre} ya contiene el tema {tema.Nombre}");
                            Console.ResetColor();
                        }
                    }
                    if (!flag_libroAgregar)
                    {
                        tema.Libros.Add(libro_seleccionado);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"\nEl libro {libro_seleccionado.Nombre} se asignó con éxito al tema {tema.Nombre}");
                        Console.ResetColor();
                    }
                    Console.ReadKey(true);

                    /*
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
                    */
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
            string[] opciones = new string[] { "Agregar", "Modificar", "Eliminar", "Listar", "Agregar Libro", "Volver" };
            Console.Clear();
            int opcion = Selection_Menu.Print("Temas", 0, opciones);
            switch (opcion)
            {
                case 0: Console.Clear(); Ordenar(); Agregar(); Menu(); break;
                case 1:
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
                    string[] nombresModificar = Program.Temas.Select(l => l.Nombre).ToArray();
                    Modificar(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombresModificar)]); Menu(); break;
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
                    string[] nombresEliminar = Program.Temas.Select(l => l.Nombre).ToArray();
                    Eliminar(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombresEliminar)]); Menu(); break;
                case 3: Console.Clear(); Ordenar(); Listar(); Menu(); break;
                case 4: Console.Clear(); Ordenar();
                    if (Program.Temas.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nNo existen datos");
                        Console.ResetColor();
                        Console.ReadKey(true);
                        Menu();
                        break;
                    }
                    string[] nombresAgregarLibro = Program.Temas.Select(l => l.Nombre).ToArray();
                    AgregarLibro(Program.Temas[Selection_Menu.Print("Lista de Temas", 0, nombresAgregarLibro)]); Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }
    }
}
