using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rootics.Core.Dtos;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;

namespace Rootics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlantsController> _logger;
        private readonly IMapper _mapper;

        public TreatmentsController(IUnitOfWork unitOfWork, ILogger<PlantsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        // GET: api/Treatments

        [HttpGet("AllTreatmentsAsync/{page}/{pageSize}")]
        public async Task<IActionResult> AllTreatmentsAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var treatments = await _unitOfWork.Treatments.GetAllAsync(page, pageSize, new[] { "PlantDisease" });

            return Ok(treatments);
        }

        [HttpGet("IdentityTreatment/{id}")]
        public async Task<IActionResult> IdentityTreatment(int? id)
        {
            if (!id.HasValue || id <= 0)
            {
                return BadRequest();
            }
            var identityTreatment = await _unitOfWork.Treatments.GetByIdAsync(id.Value);
            return Ok(identityTreatment);
        }

        // POST: api/Treatments
        [HttpPost("AddTreatment")]
        public async Task<IActionResult> AddTreatmentAsync([FromBody] TreatmentDto treatmentDto)
        {
            try
            {
                // Validate incoming data
                if (treatmentDto == null)
                {
                    return BadRequest("Treatment data is null.");
                }

                // Additional validation if needed

                // Map the DTO back to the entity model
                var treatment = _mapper.Map<Treatment>(treatmentDto);

                // Add the treatment to the repository
                var addedTreatment = await _unitOfWork.Treatments.AddAsync(treatment);

                // Save changes to the database
                await _unitOfWork.Treatments.SaveChangesAsync();

                // Map the added treatment back to a DTO
                var addedTreatmentDto = _mapper.Map<TreatmentDto>(addedTreatment);

                // Return the added treatment with a 201 Created status
                return Ok(addedTreatmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during treatment addition.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }
        // PUT: api/Treatments/UpdateTreatment
        [HttpPut("UpdateTreatment/{id}")]
        public async Task<IActionResult> UpdateTreatmentAsync(int id, [FromBody] TreatmentDto updatedTreatmentDto)
        {
            try
            {
                // Validate incoming data
                if (updatedTreatmentDto == null)
                {
                    return BadRequest("Updated treatment data is null.");
                }

                // Additional validation if needed

                // Check if the treatment with the given id exists
                var existingTreatment = await _unitOfWork.Treatments.GetByIdAsync(id);

                if (existingTreatment == null)
                {
                    return NotFound(); // Return 404 if the treatment with the given ID is not found
                }

                existingTreatment.TreatmentName = updatedTreatmentDto.TreatmentName;
                existingTreatment.Description = updatedTreatmentDto.Description;
                existingTreatment.Instructions = updatedTreatmentDto.Instructions;
                existingTreatment.PlantDiseaseId = updatedTreatmentDto.PlantDiseaseId;

                // Save changes to the database
                await _unitOfWork.Treatments.SaveChangesAsync();

                // Return the updated treatment
                return Ok(updatedTreatmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during treatment update.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }
        // DELETE: api/Treatments/DeleteTreatment/5
        [HttpDelete("DeleteTreatment/{id}")]
        public async Task<IActionResult> DeleteTreatmentAsync(int id)
        {
            try
            {
                // Check if the treatment with the given id exists
                var existingTreatment = await _unitOfWork.Treatments.GetByIdAsync(id);

                if (existingTreatment == null)
                {
                    return NotFound(); // Return 404 if the treatment with the given ID is not found
                }

                // Remove the treatment from the repository
                _unitOfWork.Treatments.Remove(existingTreatment);

                // Save changes to the database
                await _unitOfWork.Treatments.SaveChangesAsync();

                return Ok(); // Return 200 OK if the treatment is successfully deleted
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during treatment deletion.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        // Additional methods for retrieving treatment by ID, adding, or updating can be added here.
    }
}
