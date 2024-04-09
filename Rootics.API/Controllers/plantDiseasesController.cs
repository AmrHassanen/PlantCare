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
    public class plantDiseasesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<plantDiseasesController> _logger;
        private readonly IMapper _mapper;


        public plantDiseasesController(IUnitOfWork unitOfWork,ILogger<plantDiseasesController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        
        //https://localhost:7065/api/plantDiseases/GetAllAsync/1/10
        [HttpGet("GetAllAsync/{page}/{pageSize}")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var plantDiseases = await _unitOfWork.PlantDiseases.GetAllAsync(page, pageSize);

            return Ok(plantDiseases);
        }

        [HttpGet("IdentityPlantDisease/{id}")]
        public async Task<IActionResult> IdentityPlantDisease(int? id)
        {
            if (!id.HasValue || id <= 0)
            {
                return BadRequest();
            }

            var identityPlantDisease = await _unitOfWork.PlantDiseases.GetByIdAsync(id.Value);

            if (identityPlantDisease == null)
            {
                return NotFound(); // Return 404 if the plant disease with the given ID is not found
            }

            return Ok(identityPlantDisease);
        }

        [HttpPost("AddPlantDisease")]
        public async Task<IActionResult> AddPlantDiseaseAsync([FromBody] PlantDiseaseDto plantDiseaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Use AutoMapper to map PlantDiseaseDto to PlantDisease
                var plantDisease = _mapper.Map<PlantDisease>(plantDiseaseDto);

                // Add to the repository
                var addedPlantDisease = await _unitOfWork.PlantDiseases.AddAsync(plantDisease);

                // Save changes to the database
                await _unitOfWork.PlantDiseases.SaveChangesAsync();

                return Ok(addedPlantDisease);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant disease addition.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpPut("UpdatePlantDisease/{id}")]
        public async Task<IActionResult> UpdatePlantDiseaseAsync(int id,[FromBody] PlantDiseaseDto updatedPlantDiseaseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Use AutoMapper to map PlantDiseaseDto to PlantDisease
                var existingPlantDisease = await _unitOfWork.PlantDiseases.GetByIdAsync(id);

                if (existingPlantDisease == null)
                {
                    return NotFound(); // Return 404 if the plant disease with the given ID is not found
                }

                // Update the properties of the existing plant disease with the values from updatedPlantDiseaseDto
                existingPlantDisease.Name = updatedPlantDiseaseDto.Name;
                existingPlantDisease.Description = updatedPlantDiseaseDto.Description;
                existingPlantDisease.Symptoms = updatedPlantDiseaseDto.Symptoms;
                existingPlantDisease.Cause = updatedPlantDiseaseDto.Cause;
                existingPlantDisease.Prevention = updatedPlantDiseaseDto.Prevention;
                existingPlantDisease.PlantId = updatedPlantDiseaseDto.PlantId;

                // Save the changes to the database
                await _unitOfWork.PlantDiseases.SaveChangesAsync();

                return Ok(updatedPlantDiseaseDto); // Return the updated plant disease
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant disease update.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpDelete("DeletePlantDisease/{id}")]
        public async Task<IActionResult> DeletePlantDiseaseAsync(int id)
        {
            var plantDisease = await _unitOfWork.PlantDiseases.GetByIdAsync(id);

            if (plantDisease == null)
            {
                return NotFound(); // Return 404 if the plant disease with the given ID is not found
            }

            _unitOfWork.PlantDiseases.Remove(plantDisease);
            await _unitOfWork.PlantDiseases.SaveChangesAsync();

            return Ok(); // Return 200 OK if the plant disease is successfully deleted
        }


    }
}
