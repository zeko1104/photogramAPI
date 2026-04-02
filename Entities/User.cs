using Microsoft.Extensions.Hosting;

namespace PhotogramAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ProfileImagePublicId { get; set; }


        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Like> Likes { get; set; } = new();


        public List<Friendship> Followers { get; set; } = new();   
        public List<Friendship> Followings { get; set; } = new();  

    }
}
