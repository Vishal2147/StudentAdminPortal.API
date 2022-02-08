using AutoMapper;
using StudentAdminPortal.API.DomainModels;
using DataModels=  StudentAdminPortal.API.DataModels;  // to differentiate b/w Domain and Data models




namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProfiles: Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<DataModels.Student, Student>().ReverseMap();      // map dataModel and domain students models

            CreateMap<DataModels.Gender, Gender>().ReverseMap();

            CreateMap<DataModels.Address, Address>().ReverseMap();


        }
    }
}
