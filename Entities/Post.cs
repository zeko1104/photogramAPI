namespace PhotogramAPI.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Comment> Comments { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
    }
}
