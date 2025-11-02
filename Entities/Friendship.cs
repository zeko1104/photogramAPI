namespace PhotogramAPI.Entities
{
    public class Friendship
    {
        public int Id { get; set; }

        public int FollowerId { get; set; } // İzləyən istifadəçi
        public User Follower { get; set; }

        public int FollowingId { get; set; } // İzlənilən istifadəçi
        public User Following { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
