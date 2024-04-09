using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rootics.Core.Models
{
    public class Treatment
    {
        public int Id { get; set; } // Unique identifier for the treatment

        [Required(ErrorMessage = "Treatment Name is required")]
        [MaxLength(100, ErrorMessage = "Treatment Name cannot exceed 100 characters")]
        public string TreatmentName { get; set; } // Name of the treatment

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } // Description of the treatment

        [MaxLength(500, ErrorMessage = "Instructions cannot exceed 500 characters")]
        public string Instructions { get; set; } // Instructions for administering the treatment

        // Foreign key to relate Treatment to PlantDisease
        public int ?PlantDiseaseId { get; set; }

        // Navigation property for relating Treatment to a single PlantDisease
        [JsonIgnore]
        public PlantDisease PlantDisease { get; set; }

    }
}
