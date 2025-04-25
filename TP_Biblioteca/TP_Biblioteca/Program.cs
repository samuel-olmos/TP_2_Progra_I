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
            Library library = new Library();
            library.Id = 1;
            library.Name = "Test";

            //Creando escritores de libros (User Class)
            User Antoine_de_Saint_Exupéry = new User();
            Antoine_de_Saint_Exupéry.Id = 1;
            Antoine_de_Saint_Exupéry.Name = "Antoine";
            Antoine_de_Saint_Exupéry.Last_Name = "De Saint-Exupéry";
            Antoine_de_Saint_Exupéry.Email = "Antoine.Saint-Exupéry@gmail.com";

            User James_Clear = new User();
            James_Clear.Id = 2;
            James_Clear.Name = "James";
            James_Clear.Last_Name = "Clear";
            James_Clear.Email = "james.clear@gmail.com";

            User Roald_Dahl = new User();
            Roald_Dahl.Id = 3;
            Roald_Dahl.Name = "Roald";
            Roald_Dahl.Last_Name = "Dahl";
            Roald_Dahl.Email = "roald_dahl@gmail.com";

            User Bryan_Mealer = new User();
            Bryan_Mealer.Id = 4;
            Bryan_Mealer.Name = "Bryan";
            Bryan_Mealer.Last_Name = "Mealer";
            Bryan_Mealer.Email = "brymea@gmail.com";

            //Creando los Temas que ofrece la biblioteca
            Topic Literatura_Infantil = new Topic();
            Literatura_Infantil.Id = 1;
            Literatura_Infantil.Name = "Literatura Infantil";
            Literatura_Infantil.Description = "Libros escritos para el público infantil o juvenil";

            Topic Auto_Ayuda = new Topic();
            Auto_Ayuda.Id = 2;
            Auto_Ayuda.Name = "Auto-ayuda";
            Auto_Ayuda.Description = "Libros dedicados al crecimiento y desarrollo personal";

            //Creando Libros para agregar a la biblioteca
            Book El_principito = new Book();
            El_principito.Id = 1;
            El_principito.Name = "El principito";
            El_principito.Prologue = "A LEÓN WERTH\r\n\r\nPido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Tengo otra excusa: esta persona mayor puede comprender todo; hasta los libros para niños. Tengo una tercera excusa: esta persona mayor vive en Francia, donde tiene hambre y frío. Tiene verdadera necesidad de consuelo. Si todas estas excusas no fueran suficientes, quiero dedicar este libro al niño que esta persona mayor fue en otro tiempo. Todas las personas mayores han sido niños antes. (Pero pocas de ellas lo recuerdan).Corrijo, pues, mi dedicatoria:\r\n\r\nA LEÓN WERTH CUANDO ERA NIÑO.\"";
            El_principito.Writer = Antoine_de_Saint_Exupéry;
            El_principito.Topic = Literatura_Infantil;

            Book Habitos_atomicos = new Book();
            Habitos_atomicos.Id = 2;
            Habitos_atomicos.Name = "Hábitos Atómicos";
            Habitos_atomicos.Prologue = "A 3 décadas de que Stephen Covey nos revelara los 7 hábitos de la gente altamente efectiva, James Clear nos enseña la forma más sencilla y práctica de incorporar los mejores hábitos a nuestra vida diaria.";
            Habitos_atomicos.Writer = James_Clear;
            Habitos_atomicos.Topic = Auto_Ayuda;

            Book Matilda = new Book();
            Matilda.Id = 3;
            Matilda.Name = "Matilda";
            Matilda.Prologue = "Matilda no necesita presentación. ¡Ni el cine ha podido resistirse ante los encantos de este entrañable personaje! Con tan sólo cinco años, Matilda atesora unos conocimientos francamente asombrosos.";
            Matilda.Writer = Roald_Dahl;
            Matilda.Topic = Literatura_Infantil;

            Book El_niño_que_domo_el_viento = new Book();
            El_niño_que_domo_el_viento.Id = 4;
            El_niño_que_domo_el_viento.Name = "El niño que domó el viento";
            El_niño_que_domo_el_viento.Prologue = "El sueño de un niño puede cambiar el mundo entero.\r\n\r\nEsta es una inspiradora historia, basada en la vida real del autor, sobre el poder de la imaginación y la fuerza de la determinación.\r\n\r\nCuando una terrible sequía asoló la pequeña aldea donde vivía William Kamkwamba, su familia perdió todas las cosechas y se quedó sin nada que comer y nada que vender.\r\n\r\nWilliam comenzó entonces a investigar en los libros de ciencia que había en la biblioteca en busca de una solución, y de este modo encontró la idea que cambiaría la vida de su familia para siempre: construiría un molino de viento.\r\n\r\nFabricado a partir de materiales reciclados, metal y fragmentos de bicicletas, el molino de William trajo la electricidad a su casa y ayudó a su familia a obtener el agua que necesitaba para sus cultivos. Así, el empeño y la ilusión del pequeño Willy cambió el destino de su familia y del país entero.";
            El_niño_que_domo_el_viento.Writer = Bryan_Mealer;
            El_niño_que_domo_el_viento.Topic = Auto_Ayuda;

            //Añadiendo libros a la biblioteca
            library.Books_List.Add(El_principito);
            library.Books_List.Add(Habitos_atomicos);
            library.Books_List.Add(Matilda);
            library.Books_List.Add(El_niño_que_domo_el_viento);

            //Añadiendo libros a las listas de cada tópico
            /*-------------------CHEQUEAR-------------------*/
            Literatura_Infantil.Books_List.Add(El_principito);
            Literatura_Infantil.Books_List.Add(Matilda);
            Auto_Ayuda.Books_List.Add(Habitos_atomicos);
            Auto_Ayuda.Books_List.Add(El_niño_que_domo_el_viento);
            /*----------------------------------------------*/

            //Creando usuarios para los préstamos
            User user_1 = new User();
            user_1.Id = 10;
            user_1.Name = "Tomás";
            user_1.Last_Name = "Caussa";
            user_1.Email = "tomas.caussa@uap.edu.ar";

            User user_2 = new User();
            user_2.Id = 11;
            user_2.Name = "Milena";
            user_2.Last_Name = "Seri";
            user_2.Email = "milena.seri@uap.edu.ar";

            User user_3 = new User();
            user_3.Id = 12;
            user_3.Name = "Samuel";
            user_3.Last_Name = "Olmos";
            user_3.Email = "samuel.olmos@uap.edu.ar";

            //Creando Estados de préstamo de libro
            Book_Loan_Status returned = new Book_Loan_Status();
            returned.Id = 0;
            returned.Name = "En biblioteca";

            Book_Loan_Status taken = new Book_Loan_Status();
            taken.Id = 1;
            taken.Name = "Prestado";

            //Creando Préstamos
            Book_Loan loan_1 = new Book_Loan();
            loan_1.Id = 1;
            loan_1.Date = DateTime.Now;
            loan_1.Book_To_Lend = El_principito;
            loan_1.Loan_User = user_1;
            loan_1.Status = taken;

            Book_Loan loan_2 = new Book_Loan();
            loan_2.Id = 2;
            loan_2.Date = DateTime.Now;
            loan_2.Book_To_Lend = El_niño_que_domo_el_viento;
            loan_2.Loan_User = user_2;
            loan_2.Status = returned;

            Book_Loan loan_3 = new Book_Loan();
            loan_3.Id = 3;
            loan_3.Date = DateTime.Now;
            loan_3.Book_To_Lend = Habitos_atomicos;
            loan_3.Loan_User = user_3;
            loan_3.Status = taken;

        }
    }
}
