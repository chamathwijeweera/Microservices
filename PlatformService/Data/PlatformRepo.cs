using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly ApplicationDbContext dbContext;

        public PlatformRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void CreatePlatform(Platform model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            dbContext.Platforms.Add(model);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return dbContext.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return dbContext.Platforms.FirstOrDefault(platform => platform.Id == id);
        }

        public bool SaveChanges()
        {
            return dbContext.SaveChanges() >= 0;
        }
    }
}