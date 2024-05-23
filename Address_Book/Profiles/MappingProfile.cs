using Address_Book.Models;
using Address_Book.ViewModels;
using AutoMapper;
namespace Address_Book.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressBookEntry, AddressBookEntryDto>();
        CreateMap<AddressBookEntryDto, AddressBookEntry>();
    }
}