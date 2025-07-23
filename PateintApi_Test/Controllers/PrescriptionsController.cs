using Microsoft.AspNetCore.Mvc;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Interfaces;

namespace PatientAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAllPrescriptions()
        {
            var result = await _prescriptionService.GetAllPrescriptionsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> GetPrescription(int id)
        {
            var result = await _prescriptionService.GetPrescriptionByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptionsByPatientId(int patientId)
        {
            var result = await _prescriptionService.GetPrescriptionsByPatientIdAsync(patientId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> CreatePrescription(CreatePrescriptionDto createPrescriptionDto)
        {
            var result = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetPrescription), new { id = result.Value!.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePrescription(int id, UpdatePrescriptionDto updatePrescriptionDto)
        {
            var result = await _prescriptionService.UpdatePrescriptionAsync(id, updatePrescriptionDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePrescription(int id)
        {
            var result = await _prescriptionService.DeletePrescriptionAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> SearchPrescriptions([FromQuery] string drugName)
        {
            var result = await _prescriptionService.SearchPrescriptionsByDrugNameAsync(drugName);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}

