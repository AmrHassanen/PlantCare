using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Rootics.Core.Models
{
    public class CareAlertOfPlant
    {
        public int Id { get; set; } // Unique identifier for the care alert

        [Required(ErrorMessage = "Plant ID is required")]
        public int? PlantId { get; set; } // ID of the associated plant

        [Required(ErrorMessage = "Next Watering Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Next Watering Date")]
        public DateTime? NextWateringDate { get; set; } // Date for the next watering

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; } // Additional notes or instructions for caring for the plant

        [DataType(DataType.Date)]
        [Display(Name = "Last Watering Date")]
        public DateTime? LastWateringDate { get; set; } // Date of the last watering

        [Display(Name = "Watering Frequency (in days)")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive value for frequency")]
        public int ? WateringFrequencyDays { get; set; } // Frequency of watering in days

        [Display(Name = "Fertilizing Date")]
        [DataType(DataType.Date)]
        public DateTime? FertilizingDate { get; set; } // Date for fertilizing the plant

        [MaxLength(500, ErrorMessage = "Fertilizing Notes cannot exceed 500 characters")]
        public string FertilizingNotes { get; set; } // Notes or instructions for fertilizing

        // Navigation property for relating CareAlert to a single Plant
        [JsonIgnore]
        [ForeignKey("PlantId")]
        public Plant Plant { get; set; }
    }
}
