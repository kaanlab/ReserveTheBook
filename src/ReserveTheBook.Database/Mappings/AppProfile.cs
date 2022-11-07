using AutoMapper;
using ReserveTheBook.Database.Models;
using ReserveTheBook.Domain.Models;

namespace ReserveTheBook.Database.Mappings
{
    public sealed class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<AuthorDb, Author>()
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));

            CreateMap<DateTime?, DateOnly?>().ConvertUsing(new DateOnlyTypeConverter());

            CreateMap<BookDb, Book>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors));

            CreateMap<Book, BookDb>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore());

            CreateMap<ReservationDb, Reservation>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book)); //.ReverseMap();

            CreateMap<Reservation, ReservationDb>()
                .ForMember(dest => dest.Book, opt => opt.Ignore());
        }
    }

    public class DateOnlyTypeConverter : ITypeConverter<DateTime?, DateOnly?>
    {
        public DateOnly? Convert(DateTime? source, DateOnly? destination, ResolutionContext context)
        {
            if (source.HasValue)
            {
                destination = DateOnly.FromDateTime(source.Value);
                return destination;
            }

            return null;
        }
    }
}
