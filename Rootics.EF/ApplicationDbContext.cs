using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rootics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Plant> Plants { get; set; } // DbSet for the Plant model
        public DbSet<PlantDisease> PlantDiseases { get; set; } // DbSet for the PlantDisease model
        public DbSet<Treatment> Treatments { get; set; } // DbSet for the Treatment model
        public DbSet<CareAlertOfPlant> CareAlerts { get; set; } // DbSet for the CareAlertOfPlant model
    }
}
