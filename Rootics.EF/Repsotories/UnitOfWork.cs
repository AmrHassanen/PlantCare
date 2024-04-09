using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rootics.EF.Repsotories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<ApplicationUser> ApplicationUsers { get; private set; }
        public IBaseRepository<CareAlertOfPlant> CareAlertOfPlants { get; private set; }
        public IBaseRepository<Plant> Plants { get; private set; }
        public IBaseRepository<PlantDisease> PlantDiseases { get; private set; }
        public IBaseRepository<Treatment> Treatments { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ApplicationUsers = new BaseRepository<ApplicationUser>(_context);
            CareAlertOfPlants = new BaseRepository<CareAlertOfPlant>(_context);
            Plants = new BaseRepository<Plant>(_context);
            PlantDiseases = new BaseRepository<PlantDisease>(_context);
            Treatments = new BaseRepository<Treatment>(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
