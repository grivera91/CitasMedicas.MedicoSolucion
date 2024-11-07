namespace CitasMedicas.MedicoApi.DTO
{
    public class MedicoResponseDto
    {
        public int IdMedico { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoMedico { get; set; }
        public string Especialidad { get; set; }
        public string NumeroColegiatura { get; set; }        

        public List<HorarioAtencionResponseDto> HorariosAtencion { get; set; } = new List<HorarioAtencionResponseDto>();
    }
}
