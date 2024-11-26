using Seek.Contracts;

namespace Seek.Models
{
    public class CourseDetailsVM
    {
        public Course Course { get; set; }
        public Chapter? Chapter { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
