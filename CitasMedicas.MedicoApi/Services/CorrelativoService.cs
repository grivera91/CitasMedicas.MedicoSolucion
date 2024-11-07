using CitasMedicas.MedicoApi.Data;
using CitasMedicas.MedicoApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.MedicoApi.Services
{
    public class CorrelativoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CorrelativoService> _logger;

        public CorrelativoService(ApplicationDbContext context, ILogger<CorrelativoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> ObtenerNuevoCorrelativoAsync(string prefijoCorrelativo)
        {
            try
            {
                // Buscar el correlativo según el prefijo
                Correlativo? correlativo = await _context.Correlativos
                    .FirstOrDefaultAsync(c => c.Prefijo == prefijoCorrelativo);

                if (correlativo != null)
                {
                    // Incrementar el último número y actualizar la fecha de actualización
                    correlativo.UltimoNumero += 1;
                    correlativo.FechaActulizacion = DateTime.Now;

                    // Generar el nuevo código con el prefijo y el número rellenado con ceros
                    string numeroRelleno = correlativo.UltimoNumero.ToString().PadLeft(6, '0');
                    string nuevoCodigo = $"{correlativo.Prefijo}{numeroRelleno}";

                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    return nuevoCodigo;
                }

                throw new Exception("No se encontró un correlativo con el prefijo especificado.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al obtener el correlativo: {0}", ex.Message);
                throw;
            }
        }
    }
}
