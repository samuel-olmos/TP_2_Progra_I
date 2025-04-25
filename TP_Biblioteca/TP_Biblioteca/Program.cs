using System.Drawing;
using TP_Biblioteca.Models;

namespace TP_Biblioteca
{
    internal class Program
    {
        static void Main()
        {
            Cargar_Datos();
        }

        public static void Cargar_Datos()
        {
            //Creando una Biblioteca
            Biblioteca library = new Biblioteca();
            library.Id = 1;
            library.Nombre = "Test";

            //Creando escritores de libros (User Class)
            Usuario Antoine_de_Saint_Exupéry = new Usuario();
            Antoine_de_Saint_Exupéry.Id = 1;
            Antoine_de_Saint_Exupéry.Nombre = "Antoine";
            Antoine_de_Saint_Exupéry.Apellido = "De Saint-Exupéry";
            Antoine_de_Saint_Exupéry.Email = "Antoine.Saint-Exupéry@gmail.com";

            Usuario James_Clear = new Usuario();
            James_Clear.Id = 2;
            James_Clear.Nombre = "James";
            James_Clear.Apellido = "Clear";
            James_Clear.Email = "james.clear@gmail.com";

            Usuario Roald_Dahl = new Usuario();
            Roald_Dahl.Id = 3;
            Roald_Dahl.Nombre = "Roald";
            Roald_Dahl.Apellido = "Dahl";
            Roald_Dahl.Email = "roald_dahl@gmail.com";

            Usuario Bryan_Mealer = new Usuario();
            Bryan_Mealer.Id = 4;
            Bryan_Mealer.Nombre = "Bryan";
            Bryan_Mealer.Apellido = "Mealer";
            Bryan_Mealer.Email = "brymea@gmail.com";

            //Creando los Temas que ofrece la biblioteca
            Tema Literatura_Infantil = new Tema();
            Literatura_Infantil.Id = 1;
            Literatura_Infantil.Nombre = "Literatura Infantil";
            Literatura_Infantil.Descripcion = "Libros escritos para el público infantil o juvenil";

            Tema Auto_Ayuda = new Tema();
            Auto_Ayuda.Id = 2;
            Auto_Ayuda.Nombre = "Auto-ayuda";
            Auto_Ayuda.Descripcion = "Libros dedicados al crecimiento y desarrollo personal";

            //Creando Libros para agregar a la biblioteca
            Libro El_principito = new Libro();
            El_principito.Id = 1;
            El_principito.Nombre = "El principito";
            El_principito.Prologo = "A LEÓN WERTH\r\n\r\nPido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Tengo otra excusa: esta persona mayor puede comprender todo; hasta los libros para niños. Tengo una tercera excusa: esta persona mayor vive en Francia, donde tiene hambre y frío. Tiene verdadera necesidad de consuelo. Si todas estas excusas no fueran suficientes, quiero dedicar este libro al niño que esta persona mayor fue en otro tiempo. Todas las personas mayores han sido niños antes. (Pero pocas de ellas lo recuerdan).Corrijo, pues, mi dedicatoria:\r\n\r\nA LEÓN WERTH CUANDO ERA NIÑO.\"";
            El_principito.Autor = Antoine_de_Saint_Exupéry;
            //El_principito.Tema = Literatura_Infantil;

            Libro Habitos_atomicos = new Libro();
            Habitos_atomicos.Id = 2;
            Habitos_atomicos.Nombre = "Hábitos Atómicos";
            Habitos_atomicos.Prologo = "A 3 décadas de que Stephen Covey nos revelara los 7 hábitos de la gente altamente efectiva, James Clear nos enseña la forma más sencilla y práctica de incorporar los mejores hábitos a nuestra vida diaria.";
            Habitos_atomicos.Autor = James_Clear;
            //Habitos_atomicos.Tema = Auto_Ayuda;

            Libro Matilda = new Libro();
            Matilda.Id = 3;
            Matilda.Nombre = "Matilda";
            Matilda.Prologo = "Matilda no necesita presentación. ¡Ni el cine ha podido resistirse ante los encantos de este entrañable personaje! Con tan sólo cinco años, Matilda atesora unos conocimientos francamente asombrosos.";
            Matilda.Autor = Roald_Dahl;
            //Matilda.Tema = Literatura_Infantil;

            Libro El_niño_que_domo_el_viento = new Libro();
            El_niño_que_domo_el_viento.Id = 4;
            El_niño_que_domo_el_viento.Nombre = "El niño que domó el viento";
            El_niño_que_domo_el_viento.Prologo = "El sueño de un niño puede cambiar el mundo entero.\r\n\r\nEsta es una inspiradora historia, basada en la vida real del autor, sobre el poder de la imaginación y la fuerza de la determinación.\r\n\r\nCuando una terrible sequía asoló la pequeña aldea donde vivía William Kamkwamba, su familia perdió todas las cosechas y se quedó sin nada que comer y nada que vender.\r\n\r\nWilliam comenzó entonces a investigar en los libros de ciencia que había en la biblioteca en busca de una solución, y de este modo encontró la idea que cambiaría la vida de su familia para siempre: construiría un molino de viento.\r\n\r\nFabricado a partir de materiales reciclados, metal y fragmentos de bicicletas, el molino de William trajo la electricidad a su casa y ayudó a su familia a obtener el agua que necesitaba para sus cultivos. Así, el empeño y la ilusión del pequeño Willy cambió el destino de su familia y del país entero.";
            El_niño_que_domo_el_viento.Autor = Bryan_Mealer;
            //El_niño_que_domo_el_viento.Tema = Auto_Ayuda;

            //Añadiendo libros a la biblioteca
            //library.ListaLibros.Add(El_principito);
            //library.ListaLibros.Add(Habitos_atomicos);
            //library.ListaLibros.Add(Matilda);
            //library.ListaLibros.Add(El_niño_que_domo_el_viento);

            //Añadiendo libros a las listas de cada tópico
            /*-------------------CHEQUEAR-------------------*/
            Literatura_Infantil.ListaLibros.Add(El_principito);
            Literatura_Infantil.ListaLibros.Add(Matilda);
            Auto_Ayuda.ListaLibros.Add(Habitos_atomicos);
            Auto_Ayuda.ListaLibros.Add(El_niño_que_domo_el_viento);
            /*----------------------------------------------*/

            //Creando usuarios para los préstamos
            Usuario user_1 = new Usuario();
            user_1.Id = 10;
            user_1.Nombre = "Tomás";
            user_1.Apellido = "Caussa";
            user_1.Email = "tomas.caussa@uap.edu.ar";

            Usuario user_2 = new Usuario();
            user_2.Id = 11;
            user_2.Nombre = "Milena";
            user_2.Apellido = "Seri";
            user_2.Email = "milena.seri@uap.edu.ar";

            Usuario user_3 = new Usuario();
            user_3.Id = 12;
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
    }
}
