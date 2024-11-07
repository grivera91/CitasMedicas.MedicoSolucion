using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.MedicoApi.Model
{
    public class HorarioAtencion
    {
        [Key]
        public int IdHorario { get; set; }
        [ForeignKey("Medico")]
        public int IdMedico { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string UsuarioCreacion { get; set; } = "Sistema";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Propiedad de navegación para Medico
        public virtual Medico Medico { get; set; }
    }
}
