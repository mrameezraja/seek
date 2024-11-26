namespace Seek.Contracts
{
    public class Course
    {
        public string slug { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string author { get; set; }
        public List<Chapter> chapters { get; set;}
    }

    public class Chapter
    {
        public string title { get; set; }
        public string path { get; set; }
        public string slug { get; set; }
        public List<Lesson> lessons { get; set; }
    }

    public class Lesson
    {
        public string title { get; set; }
        public string path { get; set; }
        public string slug { get; set;}
    }
}
