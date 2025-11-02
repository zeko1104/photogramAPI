namespace PhotogramAPI.DTOs
{
    public class CommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
