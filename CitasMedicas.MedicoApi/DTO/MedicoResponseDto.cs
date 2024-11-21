namespace CitasMedicas.MedicoApi.DTO
{
    public class MedicoResponseDto
    {
        public int IdMedico { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public int NumeroColegiatura { get; set; }
        public string Observaciones { get; set; }

        //public List<HorarioAtencionResponseDto> HorariosAtencion { get; set; } = new List<HorarioAtencionResponseDto>();
    }
}
