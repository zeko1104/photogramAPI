namespace PhotogramAPI.Dtos
{
    public class PostFeedDto
    {
        public int PostId { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public string? AuthorProfileImage { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }
    }
}
