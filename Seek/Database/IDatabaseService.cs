using Seek.Entities;

namespace Seek.Database
{
    public interface IDatabaseService
    {
        void Init();
        Task<Setting> GetSettingsAsync(string course);
        Task UpdateSettingsAsync(Setting setting);
    }
}
