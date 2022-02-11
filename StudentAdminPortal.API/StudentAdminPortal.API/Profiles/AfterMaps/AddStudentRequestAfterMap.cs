using System;
using AutoMapper;
using StudentAdminPortal.API.DomainModels;
using Student = StudentAdminPortal.API.DataModels.Student;

namespace StudentAdminPortal.API.Profiles.AfterMaps
{
    public class AddStudentRequestAfterMap: IMappingAction<AddStudentRequest, DataModels.Student>
    {
        public void Process(AddStudentRequest source, Student destination, ResolutionContext context)
        {
            destination.Id = Guid.NewGuid();
            destination.Address = new DataModels.Address()
            {
                Id = Guid.NewGuid(),
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };

        }
    }
}
