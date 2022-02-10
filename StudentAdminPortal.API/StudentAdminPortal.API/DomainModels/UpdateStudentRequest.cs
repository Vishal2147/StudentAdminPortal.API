using System;

namespace StudentAdminPortal.API.DomainModels
{
    public class UpdateStudentRequest
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateofBirth { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }


        public Guid GenderId { get; set; }


        //Navigation Properties


        public string PhysicalAddress { get; set; }

        public string PostalAddress { get; set; }
    }
}

