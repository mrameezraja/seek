
using Seek.Contracts;

namespace Seek.Services
{
    public interface IFileService
    {
        Task<List<Course>> GetCoursesAsync();
    }
}
