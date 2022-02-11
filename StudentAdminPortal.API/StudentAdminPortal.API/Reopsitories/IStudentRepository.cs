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

        Task<List<Gender>> GetGenderAsync();

        Task<bool> Exists(Guid studentId);

       Task<Student> UpdateStudentMethod(Guid studentId, Student request);

       Task<Student> DeleteStudent(Guid studentId);

       Task<Student> AddStudentAsync(Student student);

       Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);
    }

}
