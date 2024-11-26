using Microsoft.AspNetCore.Mvc;
using Seek.Contracts;
using Seek.Database;
using Seek.Models;
using Seek.Services;

namespace Seek.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IDatabaseService _databaseService;
        public CoursesController(IFileService fileService,
            IDatabaseService databaseService)
        {
            _fileService = fileService;
            _databaseService = databaseService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new CoursesVM();

            var courses = _fileService.GetCourses();

            var settings = await _databaseService.GetSettingsAsync();

            if (settings != null)
            {
                var crs = courses.FirstOrDefault(_ => _.slug == settings.Course);

                vm.Course.Course = crs;

                if (!string.IsNullOrEmpty(settings.Chapter))
                {
                    var chpr = crs?.chapters.FirstOrDefault(_ => _.slug.Equals(settings.Chapter));

                    if (chpr != null && !string.IsNullOrEmpty(settings.Lesson))
                    {
                        var lsn = chpr.lessons.FirstOrDefault(_ => _.slug.Equals(settings.Lesson));

                        vm.Course.Chapter = chpr;
                        vm.Course.Lesson = lsn;
                    }

                }
            }

            vm.Courses = courses;
            vm.Setting = settings;

            return View(vm);
        }

        public async Task<IActionResult> Details(string course, string? chapter, string? lesson)
        {
            await _databaseService.UpdateSettingsAsync(new Entities.Setting
            {
                Course = course,
                Chapter = chapter ?? "",
                Lesson = lesson ?? ""
            });

            var vm = new CourseDetailsVM();

            var crs = _fileService.GetCourses().FirstOrDefault(_ => _.slug == course);

            vm.Course = crs;

            if (!string.IsNullOrEmpty(chapter))
            {
                var chpr = crs?.chapters.FirstOrDefault(_ => _.slug.Equals(chapter));

                if (chpr != null && !string.IsNullOrEmpty(lesson))
                {                    
                    var lsn = chpr.lessons.FirstOrDefault(_ => _.slug.Equals(lesson));

                    vm.Chapter = chpr;
                    vm.Lesson = lsn;
                }

            }   
            
            return View(vm);
        }
    }
}
