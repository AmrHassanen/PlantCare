using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.Core.Dtos
{
    public class PlantDiseaseDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } // Name of the disease

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } // Description of the disease

        [MaxLength(500, ErrorMessage = "Symptoms cannot exceed 500 characters")]
        public string Symptoms { get; set; } // Symptoms exhibited by plants affected by the disease

        [MaxLength(500, ErrorMessage = "Cause cannot exceed 500 characters")]
        public string Cause { get; set; } // Cause or source of the disease

        [MaxLength(500, ErrorMessage = "Prevention methods cannot exceed 500 characters")]
        public string Prevention { get; set; } // Prevention methods for the disease

        // Foreign key to relate PlantDisease to Plant
        public int? PlantId { get; set; }
    }
}
