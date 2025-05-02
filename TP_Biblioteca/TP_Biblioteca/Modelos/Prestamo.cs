using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Biblioteca.Modelos
{
    internal class Prestamo
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public Libro Libro { get; set; }
        // Fecha de préstamo (por defecto la fecha actual)
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        // Campo privado para la fecha límite personalizada
        private DateTime _fechaLimiteDevolucion;
        
        // Propiedad que puede ser establecida manualmente o calculada automáticamente
        public DateTime FechaLimiteDevolucion
        {
            get
            {
                // Si no fue establecida, se calcula automáticamente
                if (_fechaLimiteDevolucion == default)
                    return FechaPrestamo.AddDays(14);
                // De lo contrario, se devuelve la fecha especificada
                return _fechaLimiteDevolucion;
            }
            set
            {
                _fechaLimiteDevolucion = value;
            }
        }

        // Fecha en la que se realizó la devolución (por defecto null)
        public DateTime? FechaDevolucionReal { get; set; } = null;

        // Estado del préstamo (por defecto "Activo")
        public EstadoPrestamo Estado
        {
            get
            {
                // Si hay fecha de devolución, el estado es "Devuelto"
                if (FechaDevolucionReal.HasValue)
                    return EstadoPrestamo.Devuelto;

                // Si pasó la fecha límite y no hay fecha de devolución, el estado es "Vencido"
                if (DateTime.Now > FechaLimiteDevolucion && !FechaDevolucionReal.HasValue)
                    return EstadoPrestamo.Vencido;
                
                // En cualquier otro caso, el estado es "Activo"
                return EstadoPrestamo.Activo;
            }
        }

        public Prestamo() { }

        public Prestamo(int id, Usuario usuario, Libro libro,
            DateTime? fechaPrestamo = null,
            DateTime? fechaLimiteDevolucion = null,
            DateTime? fechaDevolucionReal = null)
        {
            Id = id;
            Usuario = usuario;
            Libro = libro;

            if (fechaPrestamo.HasValue)
                FechaPrestamo = fechaPrestamo.Value;

            if (fechaLimiteDevolucion.HasValue)
                _fechaLimiteDevolucion = fechaLimiteDevolucion.Value;

            if (fechaDevolucionReal.HasValue)
                FechaDevolucionReal = fechaDevolucionReal;
        }
    }
}
