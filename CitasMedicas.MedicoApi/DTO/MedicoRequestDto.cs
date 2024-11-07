namespace CitasMedicas.MedicoApi.DTO
{
    public class MedicoRequestDto
    {
        public int IdUsuario { get; set; }
        public string Especialidad { get; set; }
        public string NumeroColegiatura { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        public List<HorarioAtencionRequestDto> HorariosAtencion { get; set; } = new List<HorarioAtencionRequestDto>();
    }
}
