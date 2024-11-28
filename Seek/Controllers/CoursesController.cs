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

            var courses = await _fileService.GetCoursesAsync();            

            vm.Courses = courses;

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

            var crs = (await _fileService.GetCoursesAsync()).FirstOrDefault(_ => _.Slug == course);

            vm.Course = crs;

            if (!string.IsNullOrEmpty(chapter))
            {
                var chpr = crs?.Chapters.FirstOrDefault(_ => _.Slug.Equals(chapter));

                if (chpr != null && !string.IsNullOrEmpty(lesson))
                {                    
                    var lsn = chpr.Lessons.FirstOrDefault(_ => _.Slug.Equals(lesson));

                    vm.Chapter = chpr;
                    vm.Lesson = lsn;
                }

            }   
            
            return View(vm);
        }
    }
}
