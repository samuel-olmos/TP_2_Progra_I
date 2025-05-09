using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi_libreria
{
    public class Validations
    {
        public static int Numbers_only_input()
        {
            bool fin = false; //Variable para Do While
            string valor = ""; //Cadena que se almacena para return

            //"Validando" que sea un número
            do
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                //Lo que se presione no se muestra en pantalla con el True
                //Console.WriteLine((int)k.KeyChar);

                //Finalizamos el ciclo While
                if (k.Key == ConsoleKey.Enter)
                {
                    if (valor != "")
                    {
                        fin = true;
                    }
                }

                //Borrando caracteres con Backspace
                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (valor.Length > 0) //Que no borre más allá de lo que se toma por entrada
                    {
                        //Borrando el último número de la consola
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");

                        //Borrando el último número de la cadena almacenada 
                        valor = valor.Substring(0, valor.Length - 1);
                    }
                }

                //Mostrando por pantalla los valores que se están presionando (solo números)
                if ((int)k.KeyChar >= 48 && (int)k.KeyChar <= 57)
                {
                    Console.Write(k.KeyChar); //Por pantalla
                    valor = valor + k.KeyChar; //En la cadena
                }
            } while (!fin);
            Console.WriteLine();
            return int.Parse(valor); //Retorna un int
        }

        public static string Alphanumeric_input()
        {
            bool fin = false; //Variable para Do While
            string valor = ""; //Cadena que se almacena para return

            //"Validando" que sea una letra
            do
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                //Lo que se presione no se muestra en pantalla con el True
                //Console.WriteLine((int)k.KeyChar);

                if (k.Key == ConsoleKey.Enter)
                {
                    if (valor != "")
                    {
                        fin = true;
                    }

                }
                //Borrando caracteres con Backspace
                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (valor.Length > 0) //Que no borre más allá de lo que se toma por entrada
                    {
                        //Borrando la última letra de la consola
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");

                        //Borrando la última letra de la cadena almacenada 
                        valor = valor.Substring(0, valor.Length - 1);
                    }
                }

                //Mostrando por pantalla los valores que se están presionando (alfanuméricos)
                if ((int)k.KeyChar >= 32 && (int)k.KeyChar <= 254)
                {
                    Console.Write(k.KeyChar); //Por pantalla
                    valor = valor + k.KeyChar; //En la cadena
                }
            } while (!fin);
            Console.WriteLine();
            return valor.Trim(); //Retorna un string
        }

        public static string Letters_only_input()
        {
            bool fin = false; //Variable para Do While
            string valor = ""; //Cadena que se almacena para return

            //"Validando" que sea una letra
            do
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                //Lo que se presione no se muestra en pantalla con el True
                //Console.WriteLine((int)k.KeyChar);

                if (k.Key == ConsoleKey.Enter)
                {
                    if (valor != "")
                    {
                        fin = true;
                    }

                }
                //Borrando caracteres con Backspace
                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (valor.Length > 0) //Que no borre más allá de lo que se toma por entrada
                    {
                        //Borrando la última letra de la consola
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");

                        //Borrando la última letra de la cadena almacenada 
                        valor = valor.Substring(0, valor.Length - 1);
                    }
                }

                //Mostrando por pantalla los valores que se están presionando (solo letras)
                if (((int)k.KeyChar >= 65 && (int)k.KeyChar <= 90) || ((int)k.KeyChar >= 97 && (int)k.KeyChar <= 122) ||
                    ((int)k.KeyChar == 32) || ((int)k.KeyChar >= 128 && (int)k.KeyChar <= 167) || ((int)k.KeyChar >= 181 &&
                    (int)k.KeyChar <= 183) || ((int)k.KeyChar == 198) || ((int)k.KeyChar == 199) || ((int)k.KeyChar >= 210 &&
                    (int)k.KeyChar <= 218) || ((int)k.KeyChar >= 224 && (int)k.KeyChar <= 237))
                {
                    Console.Write(k.KeyChar); //Por pantalla
                    valor = valor + k.KeyChar; //En la cadena
                }
            } while (!fin);
            Console.WriteLine();
            return valor.Trim(); //Retorna un string
        }

        public static DateTime Date_input()
        {
            int dia = 0;
            int mes = 0;
            int año = 0;
            string valor = "";
            DateTime fechaValida;
            while (true)
            {
                Console.Clear();
                valor = "";
                while(true)
                {
                    ConsoleKeyInfo k = Console.ReadKey(true);

                    if (k.Key == ConsoleKey.Enter && valor.Length == 10)
                    {
                        try
                        {
                            año = int.Parse(valor.Substring(0, 4));
                            mes = int.Parse(valor.Substring(5, 2));
                            dia = int.Parse(valor.Substring(8, 2));
                            fechaValida = new DateTime(año, mes, dia);
                            return fechaValida;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("\nLa fecha ingresada no es válida. Presione una tecla para intentarlo nuevamente.");
                            Console.ReadKey(true);
                            break;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\nFormato numérico incorrecto. Presione una tecla para intentarlo nuevamente.");
                            Console.ReadKey(true);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError inesperado: {ex.Message}");
                            Console.ReadKey(true);
                            break;
                        }
                    }
                    if (k.Key == ConsoleKey.Backspace && valor.Length > 0)
                    {
                        if (valor.Length == 6) valor = valor.Substring(0, valor.Length - 2);
                        else if (valor.Length == 9) valor = valor.Substring(0, valor.Length - 2);
                        else valor = valor.Substring(0, valor.Length - 1);
                    }
                    if ((int)k.KeyChar >= 48 && (int)k.KeyChar <= 57 && valor.Length < 10)
                    {
                        if (valor.Length == 4 || valor.Length == 7) valor = valor + "/";
                        valor = valor + k.KeyChar;
                    }
                    Console.Clear();
                    Console.WriteLine(valor);
                }
            }
        }
    }

    public class Selection_Menu
    {
        public static int Print(string title, int index, string[] options)
        {
            title = title.Trim();
            if (title.Count() > 100)
            {
                Console.WriteLine("Texto demasiado largo");
                return -1;
            }
            if (index < options.Length - options.Length - 1 && index > options.Length+1)
            {
                return -1;
            }
            bool flag = true;
            string max_lenght_word = "";
            int dist = 2;
            foreach (string word in options.Append(title))
            {
                if (max_lenght_word.Length < word.Length) max_lenght_word = word;
            }
            do
            {
                Console.Clear();

                Console.Write("╔");
                if ((max_lenght_word.Length % 2 == 0 && title.Length % 2 != 0) || (max_lenght_word.Length % 2 != 0 && title.Length % 2 == 0)) dist = 3;
                for (int i = 0; i < max_lenght_word.Length+dist; i++) Console.Write("═");
                Console.WriteLine("╗");

                Console.Write("║");
                for (int i = 0; i < (max_lenght_word.Length+dist-title.Length)/2; i++) Console.Write(" ");
                Console.Write(title);
                for (int i = 0; i < (max_lenght_word.Length+dist-title.Length)/2; i++) Console.Write(" ");
                Console.WriteLine("║");

                Console.Write("╠");
                for (int i = 0; i < max_lenght_word.Length+dist; i++) Console.Write("═");
                Console.WriteLine("╣");


                foreach (string word in options)
                {
                    Console.Write("║");
                    if (word == options[index]) Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(word);
                    Console.ResetColor();
                    for (int i = 0; i < max_lenght_word.Length+dist-word.Length; i++) Console.Write(" ");
                    Console.WriteLine("║");
                }
                Console.Write("╚");
                for (int i = 0; i < max_lenght_word.Length+dist; i++) Console.Write("═");
                Console.WriteLine("╝");

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow && index >= options.Length-(options.Length-1)) index--;

                if (key.Key == ConsoleKey.DownArrow && index < options.Length-1) index++;

                if (key.Key == ConsoleKey.Enter) flag = false;
            } while (flag);
            return index;
        }
    }
}
