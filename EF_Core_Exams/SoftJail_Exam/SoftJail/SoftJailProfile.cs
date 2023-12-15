namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System.Data;
    using System.Globalization;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<DepartmentCells, Department>()
                .ForMember(x => x.Cells, y => y.MapFrom(c => c.Cells));

            this.CreateMap<CellDto, Cell>();

            this.CreateMap<PrisonerDto, Prisoner>()
                .ForMember(x => x.Mails, y => y.MapFrom(c => c.Mails))
                .ForMember(x => x.IncarcerationDate, y => y
                .MapFrom(c => DateTime.ParseExact(c.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.ReleaseDate, y => y
                .MapFrom(c => String.IsNullOrEmpty(c.ReleaseDate) ? (DateTime?)null : DateTime.ParseExact(c.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<MailDto, Mail>();

            this.CreateMap<OfficersXmlDto, Officer>()
                .ForMember(x => x.OfficerPrisoners, y => y.MapFrom(c => c.OfficerPrisoners))
                .ForMember(x => x.Position, y => y.MapFrom(c => Enum.Parse<Position>(c.Position, true)))
                .ForMember(x => x.Weapon, y => y.MapFrom(c => Enum.Parse<Weapon>(c.Weapon, true)));

            this.CreateMap<PrisonerXmlDto, OfficerPrisoner>()
                .ForMember(x => x.PrisonerId, y => y.MapFrom(c => c.Id));

            this.CreateMap<Prisoner, ExPrisonerDto>()
                .ForMember(x => x.CellNumber, y => y.MapFrom(c => c.Cell.CellNumber))
                .ForMember(x => x.TotalOfficerSalary, y => y.MapFrom(c => c.PrisonerOfficers.Sum(o => o.Officer.Salary)))
                .ForMember(x => x.Name, y => y.MapFrom(c => c.FullName));

            this.CreateMap<Officer, ExOfficerDto>()
                .ForMember(x => x.OfficerName, y => y.MapFrom(c => c.FullName))
                .ForMember(x => x.Department, y => y.MapFrom(c => c.Department.Name));
        }
    }
}
