using AutoMapper;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarPath))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.Tracks, opt => opt.Ignore())
            .ForMember(dest => dest.LikedTracks, opt => opt.Ignore());

        CreateMap<UserEntity, User>()
            .ConstructUsing(src => User.Create(src.Id, src.Name, src.Email, src.Password, src.AvatarUrl, src.EmailVerified));
    }
}
