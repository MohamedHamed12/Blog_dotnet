using AutoMapper;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>().ReverseMap();
    }
}
