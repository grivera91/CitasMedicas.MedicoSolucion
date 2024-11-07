using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.MedicoApi.Model
{
    public class Medico
    {
        [Key]
        public int IdMedico { get; set; }
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public string CodigoMedico { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string NumeroColegiatura { get; set; } = string.Empty;
        public string UsuarioCreacion { get; set; } = "Sistema";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Relación con horarios
        public List<HorarioAtencion> HorariosAtencion { get; set; } = new List<HorarioAtencion>();
    }
}
