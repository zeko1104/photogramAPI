namespace PhotogramAPI.DTOs
{
    public class UpdateProfileDto
    {
        public string Username { get; set; }
        public string? Bio { get; set; }
        public IFormFile? File { get; set; }  // optional image
    }
}
