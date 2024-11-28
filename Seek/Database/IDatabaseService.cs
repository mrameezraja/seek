using Seek.Entities;

namespace Seek.Database
{
    public interface IDatabaseService
    {
        void Init();
        Task<Setting> GetSettingsAsync();
        Task UpdateSettingsAsync(Setting setting);
    }
}
