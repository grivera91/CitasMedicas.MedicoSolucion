using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.MedicoApi.Model
{
    public class Correlativo
    {
        [Key]
        public int IdCorrelativo { get; set; }
        public string NombreCorrelativo { get; set; }
        public string Prefijo { get; set; }
        public int UltimoNumero { get; set; }
        public string? Sufijo { get; set; }
        public int Longitud { get; set; }
        public DateTime FechaActulizacion { get; set; }
    }
}
