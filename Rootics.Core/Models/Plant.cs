using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rootics.Core.Models
{
    public class Plant
    {
        public int Id { get; set; } // Unique identifier for the plant

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } // Name of the plant

        [MaxLength(100, ErrorMessage = "Species cannot exceed 100 characters")]
        public string Species { get; set; } // Species of the plant

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } // Description or details about the plant


        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } // Location or area where the plant is situated

        // Image as byte array (example for storing image data)

        [Required(ErrorMessage = "Plant image is required.")]
        [DisplayName("Plant Image")]
        public byte[] PlantImage { get; set; }
        // Navigation property for relating Plant to multiple PlantDiseases
        public ICollection<PlantDisease> PlantDiseases { get; set; }

        public ICollection<CareAlertOfPlant> CareAlertOfPlants { get; set; }


    }
}
