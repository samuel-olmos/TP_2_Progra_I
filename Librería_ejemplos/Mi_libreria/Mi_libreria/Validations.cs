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

        public static string Characters_input()
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
                if (((int)k.KeyChar >= 32 && (int)k.KeyChar <= 126) || (int)k.KeyChar == 225 || (int)k.KeyChar == 233 || (int)k.KeyChar == 237
                    || (int)k.KeyChar == 243 || (int)k.KeyChar == 250 || (int)k.KeyChar == 241 || (int)k.KeyChar == 209)
                {
                    Console.Write(k.KeyChar); //Por pantalla
                    valor = valor + k.KeyChar; //En la cadena
                }
            } while (!fin);
            Console.WriteLine();
            return valor; //Retorna un string
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

                else if (k.Key == ConsoleKey.Spacebar)
                {
                    Console.Write(" ");
                    valor = valor + " ";
                }

                //Mostrando por pantalla los valores que se están presionando (solo letras)
                if (((int)k.KeyChar >= 65 && (int)k.KeyChar <= 90) || ((int)k.KeyChar >= 97 && (int)k.KeyChar <= 122))
                {
                    Console.Write(k.KeyChar); //Por pantalla
                    valor = valor + k.KeyChar; //En la cadena
                }
            } while (!fin);
            Console.WriteLine();
            return valor; //Retorna un string
        }

        public static DateTime Date_input()
        {
            bool fin = false;
            int dia = 0;
            int mes = 0;
            int año = 0;
            string valor = "";
            do
            {
                ConsoleKeyInfo k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter && valor.Length == 10)
                {
                    fin = true;
                    año = int.Parse(valor.Substring(0, 4));
                    mes = int.Parse(valor.Substring(5, 2));
                    dia = int.Parse(valor.Substring(8, 2));
                }

                if (k.Key == ConsoleKey.Backspace && valor.Length > 0)
                {
                    Console.Clear();
                    if (valor.Length == 6) valor = valor.Substring(0, valor.Length - 2);

                    else if (valor.Length == 9) valor = valor.Substring(0, valor.Length - 2);

                    else valor = valor.Substring(0, valor.Length - 1);
                    Console.WriteLine(valor);
                }

                if ((int)k.KeyChar >= 48 && (int)k.KeyChar <= 57 && valor.Length < 10)
                {
                    Console.Clear();
                    valor = valor + k.KeyChar;
                    Console.WriteLine(valor);
                    if (valor.Length == 4) valor = valor + "/";
                    if (valor.Length == 7) valor = valor + "/";
                }

                if (valor.Length == 10)
                {
                    
                }
            } while (!fin);
            try
            {
                return new DateTime(año, mes, dia);
            }
            catch
            {
                Console.WriteLine("Ha ingresado erróneamente un dato de la fecha");
                return new DateTime(1,1,1);
            }

        }
    }

    public class Selection_Menu
    {
        public static int Print(string title, int index, string[] options)
        {
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
