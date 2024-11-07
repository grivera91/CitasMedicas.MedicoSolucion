namespace CitasMedicas.MedicoApi.DTO
{
    public class HorarioAtencionResponseDto
    {
        public int IdHorario { get; set; }
        public int IdMedico { get; set; }
        public string DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }        
    }
}
