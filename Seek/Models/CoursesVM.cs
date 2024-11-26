using Seek.Contracts;
using Seek.Entities;

namespace Seek.Models
{
    public class CoursesVM
    {
        public List<Course> Courses { get; set; }
        public Setting? Setting { get; set; }
        public CourseDetailsVM? Course { get; set; } = new CourseDetailsVM();
    }
}
