using AutoMapper;
using CPL.Domain;
using CPL.Models;

namespace CPL.Misc.AutoMapper
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactViewModel>();
        }
    }
}
