namespace LearningDapper.Models
{
    public class StudentCourse
    {
        public Course CourseId { get; set; }
        public Student StudentId { get; set; }
        public byte Progress { get; set; }
        public bool Favorite { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
