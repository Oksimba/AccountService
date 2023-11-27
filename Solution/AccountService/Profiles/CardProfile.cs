using AutoMapper;
using Entities;

namespace AccountService.Profiles
{
    public class CardProfile: Profile
    {
        public CardProfile()
        {
            CreateMap<CardCreateDto, Card>();
            CreateMap<Card?, CardDto>();
            CreateMap<CardUpdateDto, Card>();
        }
    }
}
