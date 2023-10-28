using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ApplicationDbContext context;

        public CommandRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            context.Commands.Add(command);  
           
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return context.Commands.Where(entity =>
                entity.PlatformId == platformId &&
                entity.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return context.Commands.Where(e => e.PlatformId == platformId)
                .OrderBy(entity => entity.Platform.Name);
        }

        public bool PlatformExits(int platformId)
        {
            return context.Platforms.Any(entity => entity.Id == platformId);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }
    }
}
