namespace CitasMedicas.MedicoApi.DTO
{
    public class MedicoRequestDto
    {
        public int IdUsuario { get; set; }
        public int IdEspecialidad { get; set; }
        public int NumeroColegiatura { get; set; }
        public string Observaciones { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        //public List<HorarioAtencionRequestDto> HorariosAtencion { get; set; } = new List<HorarioAtencionRequestDto>();
    }
}
