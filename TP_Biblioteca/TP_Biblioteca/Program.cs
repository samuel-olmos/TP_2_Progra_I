using System.ComponentModel.Design;
using System.Drawing;
using Mi_libreria;
using TP_Biblioteca.Controladores;
using TP_Biblioteca.Drivers;
using TP_Biblioteca.Models;

namespace TP_Biblioteca
{
    internal class Program
    {
        public static List<Libro> Libros = new List<Libro>();
        public static List<Tema> Temas = new List<Tema>();
        public static List<Usuario> Usuarios = new List<Usuario>();
        static void Main()
        {
            Cargar_Datos();
            Menu();
        }
        public static void Cargar_Datos()
        {
            //Creando los Temas que ofrece la biblioteca
            Tema Literatura_Infantil = new Tema();
            Literatura_Infantil.Id = 1;
            Literatura_Infantil.Nombre = "Literatura Infantil";
            Literatura_Infantil.Descripcion = "Libros escritos para el público infantil o juvenil";
            Temas.Add(Literatura_Infantil);

            Tema Auto_Ayuda = new Tema();
            Auto_Ayuda.Id = 2;
            Auto_Ayuda.Nombre = "Auto-ayuda";
            Auto_Ayuda.Descripcion = "Libros dedicados al crecimiento y desarrollo personal";
            Temas.Add(Auto_Ayuda);

            //Creando Libros para agregar a la biblioteca
            Libro El_principito = new Libro();
            El_principito.Id = 1;
            El_principito.Nombre = "El principito";
            El_principito.Prologo = "A LEÓN WERTH\r\n\r\nPido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Tengo otra excusa: esta persona mayor puede comprender todo; hasta los libros para niños. Tengo una tercera excusa: esta persona mayor vive en Francia, donde tiene hambre y frío. Tiene verdadera necesidad de consuelo. Si todas estas excusas no fueran suficientes, quiero dedicar este libro al niño que esta persona mayor fue en otro tiempo. Todas las personas mayores han sido niños antes. (Pero pocas de ellas lo recuerdan).Corrijo, pues, mi dedicatoria:\r\n\r\nA LEÓN WERTH CUANDO ERA NIÑO.\"";
            El_principito.Autor = "Antoine de Saint Exupéry";
            Libros.Add(El_principito);
            //El_principito.Tema = Literatura_Infantil;

            Libro Habitos_atomicos = new Libro();
            Habitos_atomicos.Id = 2;
            Habitos_atomicos.Nombre = "Hábitos Atómicos";
            Habitos_atomicos.Prologo = "A 3 décadas de que Stephen Covey nos revelara los 7 hábitos de la gente altamente efectiva, James Clear nos enseña la forma más sencilla y práctica de incorporar los mejores hábitos a nuestra vida diaria.";
            Habitos_atomicos.Autor = "James Clear";
            Libros.Add(Habitos_atomicos);
            //Habitos_atomicos.Tema = Auto_Ayuda;

            Libro Matilda = new Libro();
            Matilda.Id = 3;
            Matilda.Nombre = "Matilda";
            Matilda.Prologo = "Matilda no necesita presentación. ¡Ni el cine ha podido resistirse ante los encantos de este entrañable personaje! Con tan sólo cinco años, Matilda atesora unos conocimientos francamente asombrosos.";
            Matilda.Autor = "Roald Dhal";
            Libros.Add(Matilda);
            //Matilda.Tema = Literatura_Infantil;

            Libro El_niño_que_domo_el_viento = new Libro();
            El_niño_que_domo_el_viento.Id = 4;
            El_niño_que_domo_el_viento.Nombre = "El niño que domó el viento";
            El_niño_que_domo_el_viento.Prologo = "El sueño de un niño puede cambiar el mundo entero.\r\n\r\nEsta es una inspiradora historia, basada en la vida real del autor, sobre el poder de la imaginación y la fuerza de la determinación.\r\n\r\nCuando una terrible sequía asoló la pequeña aldea donde vivía William Kamkwamba, su familia perdió todas las cosechas y se quedó sin nada que comer y nada que vender.\r\n\r\nWilliam comenzó entonces a investigar en los libros de ciencia que había en la biblioteca en busca de una solución, y de este modo encontró la idea que cambiaría la vida de su familia para siempre: construiría un molino de viento.\r\n\r\nFabricado a partir de materiales reciclados, metal y fragmentos de bicicletas, el molino de William trajo la electricidad a su casa y ayudó a su familia a obtener el agua que necesitaba para sus cultivos. Así, el empeño y la ilusión del pequeño Willy cambió el destino de su familia y del país entero.";
            El_niño_que_domo_el_viento.Autor = "Bryan Mealer";
            Libros.Add(El_niño_que_domo_el_viento);
            //El_niño_que_domo_el_viento.Tema = Auto_Ayuda;
        
            //Añadiendo libros a las listas de cada tópico
            /*-------------------CHEQUEAR-------------------*/
        
            Literatura_Infantil.ListaLibros.Add(El_principito);
            Literatura_Infantil.ListaLibros.Add(Matilda);
            Auto_Ayuda.ListaLibros.Add(Habitos_atomicos);
            Auto_Ayuda.ListaLibros.Add(El_niño_que_domo_el_viento);
            /*----------------------------------------------*/

            //Creando usuarios para los préstamos
            Usuario user_1 = new Usuario();
            user_1.Id = 1;
            user_1.Nombre = "Tomás";
            user_1.Apellido = "Caussa";
            user_1.Email = "tomas.caussa@uap.edu.ar";

            Usuario user_2 = new Usuario();
            user_2.Id = 2;
            user_2.Nombre = "Milena";
            user_2.Apellido = "Seri";
            user_2.Email = "milena.seri@uap.edu.ar";

            Usuario user_3 = new Usuario();
            user_3.Id = 3;
            user_3.Nombre = "Samuel";
            user_3.Apellido = "Olmos";
            user_3.Email = "samuel.olmos@uap.edu.ar";

            //Creando Estados de préstamo de libro
            EstadoPrestamo returned = new EstadoPrestamo();
            returned.Id = 0;
            returned.Nombre = "En biblioteca";

            EstadoPrestamo taken = new EstadoPrestamo();
            taken.Id = 1;
            taken.Nombre = "Prestado";

            //Creando Préstamos
            Prestamo loan_1 = new Prestamo();
            loan_1.Id = 1;
            loan_1.Fecha = DateTime.Now;
            loan_1.Libro = El_principito;
            loan_1.Usuario = user_1;
            loan_1.Estado = taken;

            Prestamo loan_2 = new Prestamo();
            loan_2.Id = 2;
            loan_2.Fecha = DateTime.Now;
            loan_2.Libro = El_niño_que_domo_el_viento;
            loan_2.Usuario = user_2;
            loan_2.Estado = returned;

            Prestamo loan_3 = new Prestamo();
            loan_3.Id = 3;
            loan_3.Fecha = DateTime.Now;
            loan_3.Libro = Habitos_atomicos;
            loan_3.Usuario = user_3;
            loan_3.Estado = taken;
        }
        public static void Menu()
        {
            string[] opciones = { "Libro", "Tema", "Usuario", "Préstamo", "Salir" };
            int opcion = Selection_Menu.Print("Biblioteca", 0, opciones)+1;
            switch (opcion)
            {
                case 1: Console.Clear(); nLibro.Menu(); Menu(); break;
                case 2: Console.Clear(); nTema.Menu(); Menu(); break;
                case 3: Console.Clear(); /*nUsuario.Menu();*/ Menu(); break;
                case 4: Console.Clear(); /*nPrestamo.Menu();*/ Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }
    }
}
