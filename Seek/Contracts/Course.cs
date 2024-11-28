namespace Seek.Contracts
{
    public class Course
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Author { get; set; }
        public List<Chapter> Chapters { get; set;}
        public CourseProgress? CourseProgress { get; set; }
        public bool Started { get; set; } = false;
    }

    public class Chapter
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Slug { get; set; }
        public List<Lesson> Lessons { get; set; }
    }

    public class Lesson
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Slug { get; set;}
    }

    public class CourseProgress
    {
        public string Chapter {  get; set; }
        public string Lesson { get; set; }
    }
}
