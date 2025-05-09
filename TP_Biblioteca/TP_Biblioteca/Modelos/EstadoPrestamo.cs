using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Modelos
{
    public enum EstadoPrestamo
    {
        Pendiente = 1,  // Préstamo programado para una fecha futura
        Activo = 2,     // Libro en posesión del usuario
        Devuelto = 3,   // Libro devuelto dentro del plazo
        Vencido = 4     // No devuelto y pasada la fecha límite
    }
}