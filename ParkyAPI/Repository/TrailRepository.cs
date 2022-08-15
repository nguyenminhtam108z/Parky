using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public bool CreateTrail(Trail Trail)
        {
            _db.Trails.Add(Trail);
            return Save();
        }

        public bool DeleteTrail(Trail Trail)
        {
            _db.Trails.Remove(Trail);
            return Save();
        }

        public Trail GetTrail(int TrailId)
        {
            return _db.Trails.Include(x => x.NationalPark).FirstOrDefault(x => x.Id.Equals(TrailId));
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(x => x.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(x=>x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(x => x.Id.Equals(id));
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false; 
        }

        public bool UpdateTrail(Trail Trail)
        {
            _db.Trails.Update(Trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(x => x.NationalPark).Where(x => x.NationalParkId == npId).ToList();
        }
    }
}
