namespace VaporStore
{
    using AutoMapper;
    using System.Globalization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public class VaporStoreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public VaporStoreProfile()
        {
            this.CreateMap<UserModel, User>();

            this.CreateMap<CardModel, Card>()
                .ForMember(x => x.Type, y => y.MapFrom(x => x.Type));

        }
    }
}