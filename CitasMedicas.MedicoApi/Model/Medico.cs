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
        public int IdEspecialidad { get; set; }
        public int NumeroColegiatura { get; set; }
        public string Observaciones {  get; set; } = string.Empty;
        public string UsuarioCreacion { get; set; } = "Sistema";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }        
    }
}
