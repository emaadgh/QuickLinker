using AutoMapper;
using QuickLinker.API.Entities;
using QuickLinker.API.Models;

namespace QuickLinker.API.Profiles
{
    public class ShortenedURLProfile : Profile
    {
        public ShortenedURLProfile()
        {
            CreateMap<ShortenedURLForCreationDTO, ShortenedURL>();
        }
    }
}
