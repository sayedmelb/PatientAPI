using Microsoft.AspNetCore.Mvc;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Interfaces;

namespace PatientAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            var result = await _patientService.GetAllPatientsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var result = await _patientService.GetPatientByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto createPatientDto)
        {
            var result = await _patientService.CreatePatientAsync(createPatientDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetPatient), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(int id, UpdatePatientDto updatePatientDto)
        {
            var result = await _patientService.UpdatePatientAsync(id, updatePatientDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> SearchPatients([FromQuery] string name)
        {
            var result = await _patientService.SearchPatientsByNameAsync(name);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}/prescriptions")]
        public async Task<ActionResult<PatientWithPrescriptionsDto>> GetPatientWithPrescriptions(int id)
        {
            var result = await _patientService.GetPatientWithPrescriptionsAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
    }
}

