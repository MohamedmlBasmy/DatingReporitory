using System.Linq;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserList>()
                .ForMember(mem => mem.PhotoUrl, src => src.MapFrom(x => x.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(x => x.Age, source => source.MapFrom(fromSource => fromSource.DateOfBirth.CalculateAge()));

            CreateMap<User, UserDetail>()
                .ForMember(member => member.PhotoUrl, source => source.MapFrom(src => src.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                .ForMember(x => x.Age, src => src.MapFrom(from => from.DateOfBirth.CalculateAge()));
                
            CreateMap<Photo, PhotoDTO>();
            
            CreateMap<UserForUpdate, User>();

            CreateMap<PhotoForUpload, Photo>();

            CreateMap<UserDTO, User>();

            CreateMap<MessageForReturn,Message>().ReverseMap();

            CreateMap<Message, MessageForCreate>().ReverseMap();
        }
    }
}