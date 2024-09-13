using AutoMapper;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
         CreateMap<Comment, CommentDto>().ReverseMap();
    }
}
