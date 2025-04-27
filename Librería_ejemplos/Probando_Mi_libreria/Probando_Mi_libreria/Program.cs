using Mi_libreria;
namespace Probando_Mi_libreria
{
    internal class Program
    {
        static void Main()
        {
            
            //int num = Validations.Numbers_only_input();
            //string str = Validations.Letters_only_input();
            string[] options = { "Árbol", "Casa", "Auto", "Carpa", "Manzana", "Mano" };

            //Console.WriteLine(num);
            //Console.WriteLine(str);
            
            Console.WriteLine(options[Selection_Menu.Print("Opciones", 2, options)]);

        }
    }
}
