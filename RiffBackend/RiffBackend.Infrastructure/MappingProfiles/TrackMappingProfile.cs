using AutoMapper;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.MappingProfiles;

public class TrackMappingProfile : Profile
{
    public TrackMappingProfile()
    {
        CreateMap<TrackEntity, Track>()
            .ConstructUsing(src => Track.Create(
                src.Id,
                src.Title,
                src.Author,
                src.UserId,
                src.TrackPath,
                src.ImagePath,
                src.User != null ? MapUser(src.User) : null,
                src.CreatedAt ?? DateTime.UtcNow))
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Track, TrackEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.LikedByUsers, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }

    private static User MapUser(UserEntity entity)
    {
        return User.Create(entity.Id, entity.Name, entity.Email, entity.Password, entity.AvatarUrl, entity.EmailVerified);
    }
}
