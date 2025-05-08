using Mi_libreria;
namespace Probando_Mi_libreria
{
    internal class Program
    {
        static void Main()
        {
            
            //int num = Validations.Numbers_only_input();
            //string str = Validations.Letters_only_input();
            string[] options = { "Árbol", "Casa", "Auto", "Carpa", "Manzanas", "Mano" };

            //Console.WriteLine(num);
            //Console.WriteLine(str);
            
            //Console.WriteLine(options[Selection_Menu.Print("Opc", 0, options)]);

            DateTime date = Validations.Date_input();
            Console.WriteLine(date);
        }
    }
}
