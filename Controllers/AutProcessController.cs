using DynAmino.Dtos.Process;
using DynAmino.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DynAmino.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AutProcessController : ControllerBase
    {
        private readonly IProcessRepository _processRepository;

        public AutProcessController(IProcessRepository processRepository)
        {
            _processRepository = processRepository;
        }

        [HttpGet("processes")]
        public async Task<IActionResult> GetAllProcesses()
        {
            var processes = await _processRepository.GetAllProcessesAsync();
            return Ok(processes);
        }

        [HttpGet("processes/{id}")]
        public async Task<IActionResult> GetProcessById(int id)
        {
            var process = await _processRepository.GetProcessByIdAsync(id);
            if (process == null) return NotFound();
            return Ok(process);
        }

        [HttpPut("processes/{id}")]
        public async Task<IActionResult> UpdateProcess(int id, [FromBody] UpdateProcess updateProcess)
        {
            var requiredFields = new[] { "id", "enabled", "runTime", "frequency", "valueTime" };
            if (updateProcess == null || string.IsNullOrEmpty(updateProcess.Frequency))
                return BadRequest(new { errors = new[] { "Datos inválidos" } });
            
            if (requiredFields.Any(field => 
                updateProcess.GetType().GetProperty(field)?.GetValue(updateProcess) == null))
            {
                return BadRequest(new { errors = new[] { "Faltan campos obligatorios" } });
            }

            var existingProcess = await _processRepository.GetProcessByIdAsync(id);
            if (existingProcess == null) return NotFound();

            if (updateProcess.Frequency == "Diario")
                updateProcess.ValueTime = 0;
            else
                updateProcess.RunTime = null;

            existingProcess.Detenido = updateProcess.Enabled;
            existingProcess.Hora = updateProcess.RunTime;
            existingProcess.Periodo = updateProcess.Frequency;
            existingProcess.Valor = updateProcess.ValueTime;

            var updatedProcess = await _processRepository.UpdateProcessAsync(id, existingProcess);
            if (updatedProcess == null) return BadRequest();

            return Ok(updatedProcess);
        }

        [HttpPatch("processes/{id}/status")]
        public async Task<IActionResult> UpdateStatusProcess(int id, [FromBody] bool status)
        {
            var success = await _processRepository.UpdateStatusProcessAsync(id, status);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
