using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Reopsitories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid studentId);


    }

}
