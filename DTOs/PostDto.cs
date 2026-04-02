namespace PhotogramAPI.DTOs
{
    public class PostDto
    {
        public string Content { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
