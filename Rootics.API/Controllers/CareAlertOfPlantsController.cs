using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rootics.Core.Dtos;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace Rootics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareAlertOfPlantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CareAlertOfPlantsController> _logger;
        private readonly IMapper _mapper;

        public CareAlertOfPlantsController(IUnitOfWork unitOfWork, ILogger<CareAlertOfPlantsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("GetAllAsync/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var careAlertOfPlants = await _unitOfWork.CareAlertOfPlants.GetAllAsync(page, pageSize, new[] { "Plant" } );

            return Ok(careAlertOfPlants);
        }

        //[HttpGet("GetAllWithInclude/{page}/{pageSize}")]
        //public async Task<IActionResult> GetAllWithIncludeAsync<T>(int page = 1, int pageSize = 10)
        //{
        //    if (page <= 0 || pageSize <= 0)
        //    {
        //        return BadRequest("Invalid page or pageSize parameters.");
        //    }

        //    var careAlertOfPlants = await _unitOfWork.CareAlertOfPlants
        //        .Include(p => p.plants)
        //        .GetAllWithIncludeAsync(page, pageSize);

        //    return Ok(careAlertOfPlants);
        //}

        [HttpGet("IdentityCareAlertOfPlants/{id}")]
        public async Task<IActionResult> IdentityCareAlertOfPlants(int? id)
        {
            if (!id.HasValue || id <= 0)
            {
                return BadRequest();
            }

            var identityCareAlertOfPlants = await _unitOfWork.CareAlertOfPlants.GetByIdAsync(id.Value);

            if (identityCareAlertOfPlants == null)
            {
                return NotFound(); // Return 404 if the plant disease with the given ID is not found
            }

            return Ok(identityCareAlertOfPlants);
        }

        [HttpPost("AddCareAlertOfPlants")]
        public async Task<IActionResult> AddCareAlertOfPlantsAsync([FromBody] CareAlertOfPlantsDto careAlertOfPlantsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Use AutoMapper to map PlantDiseaseDto to PlantDisease
                var careAlertOfPlants = _mapper.Map<CareAlertOfPlant>(careAlertOfPlantsDto);

                // Add to the repository
                var addedcareAlertOfPlants = await _unitOfWork.CareAlertOfPlants.AddAsync(careAlertOfPlants);

                // Save changes to the database
                await _unitOfWork.PlantDiseases.SaveChangesAsync();

                return Ok(addedcareAlertOfPlants);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant disease addition.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("UpdateCareAlertOfPlants/{id}")]
        public async Task<IActionResult> UpdateCareAlertOfPlantsAsync(int id, [FromBody] CareAlertOfPlantsDto updatedCareAlertOfPlantsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if the provided ID is valid
                var existingCareAlertOfPlants = await _unitOfWork.CareAlertOfPlants.GetByIdAsync(id);
                if (existingCareAlertOfPlants == null)
                {
                    return NotFound($"CareAlertOfPlants with ID {id} not found.");
                }

                // Use AutoMapper to update existingCareAlertOfPlants with data from updatedCareAlertOfPlantsDto
                existingCareAlertOfPlants.PlantId = updatedCareAlertOfPlantsDto.PlantId;
                existingCareAlertOfPlants.NextWateringDate = updatedCareAlertOfPlantsDto.NextWateringDate;
                existingCareAlertOfPlants.Notes = updatedCareAlertOfPlantsDto.Notes;
                existingCareAlertOfPlants.LastWateringDate = updatedCareAlertOfPlantsDto.LastWateringDate;
                existingCareAlertOfPlants.WateringFrequencyDays = updatedCareAlertOfPlantsDto.WateringFrequencyDays;
                existingCareAlertOfPlants.FertilizingDate = updatedCareAlertOfPlantsDto.FertilizingDate;
                existingCareAlertOfPlants.FertilizingNotes = updatedCareAlertOfPlantsDto.FertilizingNotes;

                // Update in the repository
                _unitOfWork.CareAlertOfPlants.Update(existingCareAlertOfPlants);

                // Save changes to the database
                await _unitOfWork.CareAlertOfPlants.SaveChangesAsync();

                return Ok(existingCareAlertOfPlants);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An unexpected error occurred during the update of CareAlertOfPlants with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("DeleteCareAlertOfPlants/{id}")]
        public async Task<IActionResult> DeleteCareAlertOfPlantsAsync(int id)
        {
            try
            {
                // Check if the provided ID is valid
                var existingCareAlertOfPlants = await _unitOfWork.CareAlertOfPlants.GetByIdAsync(id);
                if (existingCareAlertOfPlants == null)
                {
                    return NotFound($"CareAlertOfPlants with ID {id} not found.");
                }

                // Remove from the repository
                _unitOfWork.CareAlertOfPlants.Remove(existingCareAlertOfPlants);

                // Save changes to the database
                await _unitOfWork.CareAlertOfPlants.SaveChangesAsync();

                return Ok($"CareAlertOfPlants with ID {id} has been deleted.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An unexpected error occurred during the deletion of CareAlertOfPlants with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
