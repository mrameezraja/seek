using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Seek.Contracts;
using Slugify;
using System.IO;

namespace Seek.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IConfiguration _configuration;
        private readonly List<Course> _courses;
        private readonly IMemoryCache _cache;
        private readonly ISlugHelper _slugHelper;
        public FileService(ILogger<FileService> logger,
            IConfiguration configuration,
            IOptions<List<Course>> options,
            IMemoryCache cache,
            ISlugHelper slugHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _courses = options.Value;
            _cache = cache;
            _slugHelper = slugHelper;
        }

        public List<Course> GetCourses()
        {
            var getCachedCourses = () =>
            {
                foreach (var course in _courses)
                {
                    course.chapters = GetChapters(course);
                }

                return _courses;
            };

            var cacheKey = "_courses";

            var courses = _cache.Get<List<Course>>(cacheKey);

            if (courses == null)
            {
                courses = getCachedCourses();
                _cache.Set<List<Course>>(cacheKey, courses, new MemoryCacheEntryOptions
                {
                    Size = int.MaxValue
                });
            }

            return courses;
        }

        public List<Chapter> GetChapters(Course course)
        {
            var directory = new DirectoryInfo(course.path);

            var chapters = directory.GetDirectories()
            .OrderBy(_ => _.Name)
            .Select(_ => new Chapter 
            { 
                title = _.Name, 
                path = _.FullName,
                slug = _slugHelper.GenerateSlug(_.Name),
                lessons = GetLessons(course, _.FullName)
            }).ToList();

            return chapters;
        }

        public List<Lesson> GetLessons(Course course, string chapterPath)
        {
            var directory = new DirectoryInfo(chapterPath);
            const string ext = ".mp4";
            var lessons = directory.GetFiles()
                .Where(_ => _.Name.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                .OrderBy(_ => _.Name)
                .Select(_ => new Lesson
                {
                    title = _.Name.Replace(ext, ""),
                    path = _.FullName,
                    slug = _slugHelper.GenerateSlug(_.Name)
                })
                .ToList();

            return lessons;
        }

    }
}
