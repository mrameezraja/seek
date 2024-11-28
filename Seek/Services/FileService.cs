using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Seek.Contracts;
using Seek.Database;
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
        private readonly IDatabaseService _databaseService;
        public FileService(ILogger<FileService> logger,
            IConfiguration configuration,
            IOptions<List<Course>> options,
            IMemoryCache cache,
            ISlugHelper slugHelper,
            IDatabaseService databaseService)
        {
            _logger = logger;
            _configuration = configuration;
            _courses = options.Value;
            _cache = cache;
            _slugHelper = slugHelper;
            _databaseService = databaseService;
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var getCachedCourses = async () =>
            {
                foreach (var course in _courses)
                {
                    course.Chapters = GetChapters(course);

                    course.CourseProgress = new CourseProgress();

                    var settings = await _databaseService.GetSettingsAsync(course.Slug);

                    course.CourseProgress = new CourseProgress
                    {
                        Chapter = settings?.Chapter,
                        Lesson = settings?.Lesson
                    };

                    course.Started = settings != null;                    
                }

                return _courses;
            };

            var cacheKey = "_courses";

            var courses = _cache.Get<List<Course>>(cacheKey);

            if (courses == null)
            {
                courses = await getCachedCourses();
                _cache.Set<List<Course>>(cacheKey, courses, new MemoryCacheEntryOptions
                {
                    Size = int.MaxValue
                });
            }

            return courses;
        }

        public List<Chapter> GetChapters(Course course)
        {
            var directory = new DirectoryInfo(course.Path);

            var chapters = directory.GetDirectories()
            .OrderBy(_ => _.Name)
            .Select(_ => new Chapter 
            { 
                Title = _.Name, 
                Path = _.FullName,
                Slug = _slugHelper.GenerateSlug(_.Name),
                Lessons = GetLessons(course, _.FullName)
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
                    Title = _.Name.Replace(ext, ""),
                    Path = _.FullName,
                    Slug = _slugHelper.GenerateSlug(_.Name)
                })
                .ToList();

            return lessons;
        }

    }
}
