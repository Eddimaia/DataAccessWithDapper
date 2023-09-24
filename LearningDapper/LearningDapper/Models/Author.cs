namespace LearningDapper.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public byte Type { get; set; }
    }
}