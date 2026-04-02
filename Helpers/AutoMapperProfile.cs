using AutoMapper;
using PhotogramAPI.Dtos;
using PhotogramAPI.Entities;

namespace PhotogramAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostFeedDto>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.AuthorProfileImage, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));
        }
    }
}
