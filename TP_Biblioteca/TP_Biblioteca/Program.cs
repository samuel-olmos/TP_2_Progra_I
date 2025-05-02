using System.ComponentModel.Design;
using System.Drawing;
using Mi_libreria;
using TP_Biblioteca.Controladores;
using TP_Biblioteca.Modelos;

namespace TP_Biblioteca
{
    internal class Program
    {
        public static List<Libro> Libros = new List<Libro>();
        public static List<Tema> Temas = new List<Tema>();
        public static List<Usuario> Usuarios = new List<Usuario>();
        public static List<Prestamo> Prestamos = new List<Prestamo>();

        static void Main()
        {
            CargarDatos();
            Menu();
        }

        public static void CargarDatos()
        {
            // Creación de temas
            var tema1 = new Tema {
                Id = 1,
                Nombre = "Literatura infantil"
            };
            Temas.Add(tema1);

            var tema2 = new Tema {
                Id = 2,
                Nombre = "Autoayuda"
            };
            Temas.Add(tema2);

            var tema3 = new Tema
            {
                Id = 3,
                Nombre = "Novelas cortas"
            };
            Temas.Add(tema3);

            var tema4 = new Tema
            {
                Id = 4,
                Nombre = "Biografía"
            };
            Temas.Add(tema4);

            // Creación de libros
            var libro1 = new Libro {
                Id = 1,
                Nombre = "El Principito",
                Prologo = "A LEÓN WERTH\r\n\r\nPido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Tengo otra excusa: esta persona mayor puede comprender todo; hasta los libros para niños. Tengo una tercera excusa: esta persona mayor vive en Francia, donde tiene hambre y frío. Tiene verdadera necesidad de consuelo. Si todas estas excusas no fueran suficientes, quiero dedicar este libro al niño que esta persona mayor fue en otro tiempo. Todas las personas mayores han sido niños antes. (Pero pocas de ellas lo recuerdan).Corrijo, pues, mi dedicatoria:\r\n\r\nA LEÓN WERTH CUANDO ERA NIÑO.\"",
                Autor = "Antoine de Saint Exupéry",
                Temas = { tema1, tema3 }
            };
            Libros.Add(libro1);
            tema1.Libros.Add(libro1);
            tema3.Libros.Add(libro1);

            var libro2 = new Libro {
                Id = 2,
                Nombre = "Hábitos Atómicos",
                Prologo = "A 3 décadas de que Stephen Covey nos revelara los 7 hábitos de la gente altamente efectiva, James Clear nos enseña la forma más sencilla y práctica de incorporar los mejores hábitos a nuestra vida diaria.",
                Autor = "James Clear",
                Temas = { tema2 }
            };
            Libros.Add(libro2);
            tema2.Libros.Add(libro2);

            var libro3 = new Libro {
                Id = 3,
                Nombre = "Matilda",
                Prologo = "Matilda no necesita presentación. ¡Ni el cine ha podido resistirse ante los encantos de este entrañable personaje! Con tan sólo cinco años, libro3 atesora unos conocimientos francamente asombrosos.",
                Autor = "Roald Dhal",
                Temas = { tema1 }
            };
            Libros.Add(libro3);
            tema1.Libros.Add(libro3);

            Libro libro4 = new Libro {
                Id = 4,
                Nombre = "El niño que domó el viento",
                Prologo = "El sueño de un niño puede cambiar el mundo entero.\r\n\r\nEsta es una inspiradora historia, basada en la vida real del autor, sobre el poder de la imaginación y la fuerza de la determinación.\r\n\r\nCuando una terrible sequía asoló la pequeña aldea donde vivía William Kamkwamba, su familia perdió todas las cosechas y se quedó sin nada que comer y nada que vender.\r\n\r\nWilliam comenzó entonces a investigar en los libros de ciencia que había en la biblioteca en busca de una solución, y de este modo encontró la idea que cambiaría la vida de su familia para siempre: construiría un molino de viento.\r\n\r\nFabricado a partir de materiales reciclados, metal y fragmentos de bicicletas, el molino de William trajo la electricidad a su casa y ayudó a su familia a obtener el agua que necesitaba para sus cultivos. Así, el empeño y la ilusión del pequeño Willy cambió el destino de su familia y del país entero.",
                Autor = "Bryan Mealer",
                Temas = { tema4 }
            };
            Libros.Add(libro4);
            tema4.Libros.Add(libro4);

            // Creación de usuarios
            var usuario1 = new Usuario {
                Id = 1,
                Nombre = "Tomás",
                Apellido = "Caussa",
                Email = "tomas.caussa@uap.edu.ar"
            };
            Usuarios.Add(usuario1);

            var usuario2 = new Usuario {
                Id = 2,
                Nombre = "Milena",
                Apellido = "Seri",
                Email = "milena.seri@uap.edu.ar"
            };
            Usuarios.Add(usuario2);

            var usuario3 = new Usuario {
                Id = 3,
                Nombre = "Samuel",
                Apellido = "Olmos",
                Email = "samuel.olmos@uap.edu.ar"
            };
            Usuarios.Add(usuario3);

            // Creación de préstamos
            var prestamo1 = new Prestamo {
                Id = 1,
                Usuario = usuario1,
                Libro = libro1
                // Sin fecha de préstamo (DateTime.Now)
                // Sin fecha límite (DateTime.Now + 14 días)
                // Sin fecha de devolución (null)
                // Estado "Activo"
            };
            Prestamos.Add(prestamo1);

            var prestamo2 = new Prestamo {
                Id = 2,
                Usuario = usuario2,
                Libro = libro3,
                FechaPrestamo = new DateTime(2025, 3, 1),
                FechaLimiteDevolucion = new DateTime(2025, 3, 21), // Extendida
                FechaDevolucionReal = new DateTime(2025, 3, 15) // Devuelto
                // Estado "Devuelto"
            };
            Prestamos.Add(prestamo2);

            var prestamo3 = new Prestamo {
                Id = 3,
                Usuario = usuario3,
                Libro = libro2,
                FechaPrestamo = new DateTime(2025, 2, 4)
                // Sin fecha límite (DateTime.Now + 14 días)
                // Sin fecha de devolución (null)
                // Estado "Vencido"
            };
            Prestamos.Add(prestamo3);
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
                case 4: Console.Clear(); nPrestamo.Menu(); Menu(); break;
                case 5: break;
                default: Menu(); break;
            }
        }
    }
}
