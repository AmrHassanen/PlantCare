using Rootics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.Core.InterFaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ApplicationUser> ApplicationUsers { get; }
        IBaseRepository<CareAlertOfPlant> CareAlertOfPlants { get; }
        IBaseRepository<Plant> Plants { get; }
        IBaseRepository<PlantDisease> PlantDiseases { get; }
        IBaseRepository<Treatment> Treatments { get; }
        int Complete();

    }
}
