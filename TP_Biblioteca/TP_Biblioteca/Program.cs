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
            CargarDatos();
            Menu();
        }

        public static void CargarDatos()
        {
            // Creación de temas
            Tema tema1 = new Tema();
            tema1.Id = 1;
            tema1.Nombre = "Literatura Infantil";
            tema1.Descripcion = "Libros escritos para el público infantil o juvenil";
            Temas.Add(tema1);

            Tema tema2 = new Tema();
            tema2.Id = 2;
            tema2.Nombre = "Auto-ayuda";
            tema2.Descripcion = "Libros dedicados al crecimiento y desarrollo personal";
            Temas.Add(tema2);

            // Creación de libros
            Libro libro1 = new Libro();
            libro1.Id = 1;
            libro1.Nombre = "El Principito";
            libro1.Prologo = "A LEÓN WERTH\r\n\r\nPido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Tengo otra excusa: esta persona mayor puede comprender todo; hasta los libros para niños. Tengo una tercera excusa: esta persona mayor vive en Francia, donde tiene hambre y frío. Tiene verdadera necesidad de consuelo. Si todas estas excusas no fueran suficientes, quiero dedicar este libro al niño que esta persona mayor fue en otro tiempo. Todas las personas mayores han sido niños antes. (Pero pocas de ellas lo recuerdan).Corrijo, pues, mi dedicatoria:\r\n\r\nA LEÓN WERTH CUANDO ERA NIÑO.\"";
            libro1.Autor = "Antoine de Saint Exupéry";
            Libros.Add(libro1);
            //libro1.Tema = tema1;

            Libro libro2 = new Libro();
            libro2.Id = 2;
            libro2.Nombre = "Hábitos Atómicos";
            libro2.Prologo = "A 3 décadas de que Stephen Covey nos revelara los 7 hábitos de la gente altamente efectiva, James Clear nos enseña la forma más sencilla y práctica de incorporar los mejores hábitos a nuestra vida diaria.";
            libro2.Autor = "James Clear";
            Libros.Add(libro2);
            //libro2.Tema = tema2;

            Libro libro3 = new Libro();
            libro3.Id = 3;
            libro3.Nombre = "Matilda";
            libro3.Prologo = "Matilda no necesita presentación. ¡Ni el cine ha podido resistirse ante los encantos de este entrañable personaje! Con tan sólo cinco años, libro3 atesora unos conocimientos francamente asombrosos.";
            libro3.Autor = "Roald Dhal";
            Libros.Add(libro3);
            //libro3.Tema = tema1;

            Libro libro4 = new Libro();
            libro4.Id = 4;
            libro4.Nombre = "El niño que domó el viento";
            libro4.Prologo = "El sueño de un niño puede cambiar el mundo entero.\r\n\r\nEsta es una inspiradora historia, basada en la vida real del autor, sobre el poder de la imaginación y la fuerza de la determinación.\r\n\r\nCuando una terrible sequía asoló la pequeña aldea donde vivía William Kamkwamba, su familia perdió todas las cosechas y se quedó sin nada que comer y nada que vender.\r\n\r\nWilliam comenzó entonces a investigar en los libros de ciencia que había en la biblioteca en busca de una solución, y de este modo encontró la idea que cambiaría la vida de su familia para siempre: construiría un molino de viento.\r\n\r\nFabricado a partir de materiales reciclados, metal y fragmentos de bicicletas, el molino de William trajo la electricidad a su casa y ayudó a su familia a obtener el agua que necesitaba para sus cultivos. Así, el empeño y la ilusión del pequeño Willy cambió el destino de su familia y del país entero.";
            libro4.Autor = "Bryan Mealer";
            Libros.Add(libro4);
            //libro4.Tema = tema2;
        
            // Asignación de libros a temas
            /*-------------------CHEQUEAR-------------------*/
        
            tema1.ListaLibros.Add(libro1);
            tema1.ListaLibros.Add(libro3);
            tema2.ListaLibros.Add(libro2);
            tema2.ListaLibros.Add(libro4);
            /*----------------------------------------------*/

            // Creación de usuarios
            Usuario usuario1 = new Usuario();
            usuario1.Id = 1;
            usuario1.Nombre = "Tomás";
            usuario1.Apellido = "Caussa";
            usuario1.Email = "tomas.caussa@uap.edu.ar";

            Usuario usuario2 = new Usuario();
            usuario2.Id = 2;
            usuario2.Nombre = "Milena";
            usuario2.Apellido = "Seri";
            usuario2.Email = "milena.seri@uap.edu.ar";

            Usuario usuario3 = new Usuario();
            usuario3.Id = 3;
            usuario3.Nombre = "Samuel";
            usuario3.Apellido = "Olmos";
            usuario3.Email = "samuel.olmos@uap.edu.ar";

            // Creación de estados de un préstamo
            EstadoPrestamo estado1 = new EstadoPrestamo();
            estado1.Id = 0;
            estado1.Nombre = "Devuelto";

            EstadoPrestamo estado2 = new EstadoPrestamo();
            estado2.Id = 1;
            estado2.Nombre = "Prestado";

            // Creación de préstamos
            Prestamo prestamo1 = new Prestamo();
            prestamo1.Id = 1;
            prestamo1.Fecha = DateTime.Now;
            prestamo1.Libro = libro1;
            prestamo1.Usuario = usuario1;
            prestamo1.Estado = estado2;

            Prestamo prestamo2 = new Prestamo();
            prestamo2.Id = 2;
            prestamo2.Fecha = DateTime.Now;
            prestamo2.Libro = libro4;
            prestamo2.Usuario = usuario2;
            prestamo2.Estado = estado1;

            Prestamo prestamo3 = new Prestamo();
            prestamo3.Id = 3;
            prestamo3.Fecha = DateTime.Now;
            prestamo3.Libro = libro2;
            prestamo3.Usuario = usuario3;
            prestamo3.Estado = estado2;
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
