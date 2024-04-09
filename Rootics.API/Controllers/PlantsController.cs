using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Rootics.Core.Dtos;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using System.Reflection.Metadata;

namespace Rootics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PlantsController> _logger;
        private readonly IMapper _mapper;
        private List<string> _allowExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        private long _maxAllowedImageSize = 10485760;
        public PlantsController(IUnitOfWork unitOfWork, ILogger<PlantsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("AllPlantsAsync/{page}/{pageSize}")]
        public async Task<IActionResult> AllPlantsAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var Plants = await _unitOfWork.Plants.GetAllAsync(page, pageSize);

            return Ok(Plants);
        }
        [HttpGet("IdentityPlant/{id}")]
        public async Task<IActionResult> IdentityPlant(int? id)
        {
            if  (!id.HasValue || id <= 0)
            {
                return BadRequest();
            }
            var identityPlants = await _unitOfWork.Plants.GetByIdAsync(id.Value);
            return Ok(identityPlants);
        }

        [HttpPost("AddPlant")]
        public async Task<IActionResult> AddPlantAsync([FromForm] PlantDto plantDto)
        {
            try
            {
                // Validate incoming data
                if (plantDto == null)
                {
                    return BadRequest("Plant data is null.");
                }

                // Additional validation if needed
                if (string.IsNullOrEmpty(plantDto.Name))
                {
                    ModelState.AddModelError("Name", "Plant name is required.");
                }

                // Check model state for validation errors
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check file extension
                string fileExtension = Path.GetExtension(plantDto.PlantImage.FileName);
                if (!_allowExtensions.Contains(fileExtension.ToLower()))
                {
                    ModelState.AddModelError("PlantImage", "Invalid file extension. Allowed extensions are: " +
                                                          string.Join(", ", _allowExtensions));
                    return BadRequest(ModelState);
                }

                // Check file size
                if (plantDto.PlantImage.Length > _maxAllowedImageSize)
                {
                    ModelState.AddModelError("PlantImage", $"File size exceeds the maximum allowed size of {(_maxAllowedImageSize / 1024) - 240} MBs.");
                    return BadRequest(ModelState);
                }

                // Convert IFormFile to byte[]
                using var dataStream = new MemoryStream();
                await plantDto.PlantImage.CopyToAsync(dataStream);

                // Create Plant object
                var plant = new Plant
                {
                    Name = plantDto.Name,
                    Species = plantDto.Species,
                    Description = plantDto.Description,
                    Location = plantDto.Location,
                    PlantImage = dataStream.ToArray(),
                };

                // Add the plant to the repository
                var addedPlant = await _unitOfWork.Plants.AddAsync(plant);

                // Save changes to the database
                await _unitOfWork.Plants.SaveChangesAsync();

                // Return the added plant with a 201 Created status
                return Ok(addedPlant);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant addition.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpPut("UpdatePlant/{id}")]
        public async Task<IActionResult> UpdatePlantAsync(int id, [FromForm] PlantDto updatedPlant)
        {
            try
            {
                // Validate incoming data
                if (updatedPlant == null)
                {
                    return BadRequest("Updated plant data is null.");
                }

                // Additional validation if needed
                if (string.IsNullOrEmpty(updatedPlant.Name))
                {
                    ModelState.AddModelError("Name", "Plant name is required.");
                }

                // Check model state for validation errors
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the plant with the given id exists
                var existingPlant = await _unitOfWork.Plants.GetByIdAsync(id);
                if (existingPlant == null)
                {
                    return NotFound(); // Return 404 if the plant with the given ID is not found
                }

                // Check file extension
                string fileExtension = Path.GetExtension(updatedPlant.PlantImage.FileName);
                if (!_allowExtensions.Contains(fileExtension.ToLower()))
                {
                    ModelState.AddModelError("PlantImage", "Invalid file extension. Allowed extensions are: " +
                                                          string.Join(", ", _allowExtensions));
                    return BadRequest(ModelState);
                }

                // Check file size
                if (updatedPlant.PlantImage.Length > _maxAllowedImageSize)
                {
                    ModelState.AddModelError("PlantImage", $"File size exceeds the maximum allowed size of {(_maxAllowedImageSize/1024)-240} MBs.");
                    return BadRequest(ModelState);
                }

                // Convert IFormFile to byte[]
                using var dataStream = new MemoryStream();
                await updatedPlant.PlantImage.CopyToAsync(dataStream);

                // Update only the properties that are intended to be updated
                existingPlant.Name = updatedPlant.Name;
                existingPlant.Species = updatedPlant.Species;
                existingPlant.Description = updatedPlant.Description;
                existingPlant.Location = updatedPlant.Location;
                existingPlant.PlantImage = dataStream.ToArray();

                // Save changes to the database
                await _unitOfWork.Plants.SaveChangesAsync();

                // Return the updated plant
                return Ok(existingPlant);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant update.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpDelete("DeletePlant/{id}")]
        public async Task<IActionResult> DeletePlantAsync(int id)
        {
            try
            {
                // Check if the plant with the given id exists
                var existingPlant = await _unitOfWork.Plants.GetByIdAsync(id);
                if (existingPlant == null)
                {
                    return NotFound(); // Return 404 if the plant with the given ID is not found
                }

                // Remove the plant from the repository
                _unitOfWork.Plants.Remove(existingPlant);

                // Save changes to the database
                await _unitOfWork.Plants.SaveChangesAsync();

                // Return 204 No Content as the plant is successfully deleted
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unexpected error occurred during plant deletion.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
