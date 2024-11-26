using Seek.Entities;

namespace Seek.Database
{
    public interface IDatabaseService
    {
        Task<Setting> GetSettingsAsync();
        Task UpdateSettingsAsync(Setting setting);
    }
}
