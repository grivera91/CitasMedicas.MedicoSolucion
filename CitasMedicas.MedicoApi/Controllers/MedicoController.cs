using CitasMedicas.MedicoApi.Data;
using CitasMedicas.MedicoApi.DTO;
using CitasMedicas.MedicoApi.Model;
using CitasMedicas.MedicoApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.MedicoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CorrelativoService _correlativoService;

        public MedicoController(ApplicationDbContext context, CorrelativoService correlativoService)
        {
            _context = context;
            _correlativoService = correlativoService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegistrarMedico([FromBody] MedicoRequestDto medicoRequestDto)
        {
            if (!ModelState.IsValid)
            {                
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Iniciar una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string codigoMedico = await _correlativoService.ObtenerNuevoCorrelativoAsync("CM");

                Medico medico = new Medico
                {
                    IdUsuario = medicoRequestDto.IdUsuario,
                    CodigoMedico = codigoMedico,
                    Especialidad = medicoRequestDto.Especialidad,
                    NumeroColegiatura = medicoRequestDto.NumeroColegiatura,
                    UsuarioCreacion = medicoRequestDto.UsuarioCreacion,
                    FechaCreacion = DateTime.Now
                };

                // Guardar el médico en la base de datos
                _context.Medicos.Add(medico);
                await _context.SaveChangesAsync();

                MedicoResponseDto medicoResponseDto = new MedicoResponseDto
                {
                    IdMedico = medico.IdMedico,
                    IdUsuario = medico.IdUsuario,
                    CodigoMedico = medico.CodigoMedico,
                    Especialidad = medico.Especialidad,
                    NumeroColegiatura = medico.NumeroColegiatura,
                    HorariosAtencion = new List<HorarioAtencionResponseDto>()
                };

                // Guardar cada horario asociado al médico
                if (medicoRequestDto.HorariosAtencion != null && medicoRequestDto.HorariosAtencion.Any())
                {
                    List<HorarioAtencion> horariosAtencion = new List<HorarioAtencion>();

                    foreach (HorarioAtencionRequestDto horarioAtencionRequestDto in medicoRequestDto.HorariosAtencion)
                    {
                        HorarioAtencion horarioAtencion = new HorarioAtencion
                        {
                            IdMedico = medico.IdMedico,
                            DiaSemana = horarioAtencionRequestDto.DiaSemana,
                            HoraInicio = horarioAtencionRequestDto.HoraInicio,
                            HoraFin = horarioAtencionRequestDto.HoraFin,
                            UsuarioCreacion = medico.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,
                        };

                        horariosAtencion.Add(horarioAtencion);
                        medicoResponseDto.HorariosAtencion.Add(new HorarioAtencionResponseDto
                        {
                            IdHorario = horarioAtencion.IdHorario,
                            IdMedico = horarioAtencion.IdMedico,
                            DiaSemana = horarioAtencion.DiaSemana,
                            HoraInicio = horarioAtencion.HoraInicio,
                            HoraFin = horarioAtencion.HoraFin
                        });
                    }

                    _context.HorariosAtencion.AddRange(horariosAtencion);
                    await _context.SaveChangesAsync();
                }

                // Confirmar la transacción
                await transaction.CommitAsync();                

                return Ok(medicoResponseDto);
            }
            catch (Exception ex)
            {
                // Deshacer la transacción en caso de error
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditarMedico(int id, [FromBody] MedicoRequestDto medicoRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Medico? medico = await _context.Medicos.Include(m => m.HorariosAtencion).FirstOrDefaultAsync(m => m.IdMedico == id);

                if (medico == null)
                {
                    return NotFound(); // Retorna 404 si el médico no existe
                }

                // Actualizar los campos del médico solo si se proporcionan en MedicoRequestDto
                if (!string.IsNullOrEmpty(medicoRequest.Especialidad))
                {
                    medico.Especialidad = medicoRequest.Especialidad;
                }

                if (!string.IsNullOrEmpty(medicoRequest.NumeroColegiatura))
                {
                    medico.NumeroColegiatura = medicoRequest.NumeroColegiatura;
                }

                medico.UsuarioModificacion = medicoRequest.UsuarioModificacion;
                medico.FechaModificacion = DateTime.Now;

                // Actualizar los horarios de atención
                if (medicoRequest.HorariosAtencion != null && medicoRequest.HorariosAtencion.Any())
                {
                    // Eliminar los horarios existentes
                    _context.HorariosAtencion.RemoveRange(medico.HorariosAtencion);

                    // Agregar los nuevos horarios
                    foreach (var horarioDto in medicoRequest.HorariosAtencion)
                    {
                        HorarioAtencion? nuevoHorario = new HorarioAtencion
                        {
                            IdMedico = medico.IdMedico,
                            DiaSemana = horarioDto.DiaSemana,
                            HoraInicio = horarioDto.HoraInicio,
                            HoraFin = horarioDto.HoraFin,
                            UsuarioCreacion = horarioDto.UsuarioCreacion,
                            FechaCreacion = DateTime.Now,                            
                        };

                        medico.HorariosAtencion.Add(nuevoHorario);
                    }
                }

                // Guardar los cambios en la base de datos
                _context.Medicos.Update(medico);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); // Confirma la transacción si todo fue exitoso

                return NoContent(); // Retorna 204 No Content si la actualización fue exitosa
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Revertir la transacción en caso de error
                return StatusCode(500, "Error al actualizar el médico y sus horarios"); // Retorna un código 500 en caso de error
            }
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoResponseDto>> ObtenerMedico(int id)
        {
            var medico = await _context.Medicos
                .Where(m => m.IdMedico == id)
                .Select(m => new MedicoResponseDto
                {
                    IdMedico = m.IdMedico,
                    IdUsuario = m.IdUsuario,
                    CodigoMedico = m.CodigoMedico,
                    Especialidad = m.Especialidad,
                    NumeroColegiatura = m.NumeroColegiatura,
                    HorariosAtencion = m.HorariosAtencion.Select(h => new HorarioAtencionResponseDto
                    {
                        IdHorario = h.IdHorario,
                        IdMedico = h.IdMedico,
                        DiaSemana = h.DiaSemana,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin                        
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (medico == null)
            {
                return NotFound();
            }

            return Ok(medico);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoResponseDto>>> ObtenerMedicos()
        {
            var medicos = await _context.Medicos
                .Select(m => new    
                {
                    IdMedico = m.IdMedico,
                    IdUsuario = m.IdUsuario,
                    CodigoMedico = m.CodigoMedico,
                    Especialidad = m.Especialidad,
                    NumeroColegiatura = m.NumeroColegiatura                    
                })
                .ToListAsync();

            return Ok(medicos);
        }
    }
}
