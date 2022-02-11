using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Reopsitories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext _context;   // use _context to talk to db in this page only.


        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync(); // gives list of students from db & include Navigation property

        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address))
                .FirstOrDefaultAsync(x => x.Id==studentId);
        }

        public async Task<List<Gender>> GetGenderAsync()
        {
             return await _context.Gender.ToListAsync();
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await _context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<Student> UpdateStudentMethod(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);

            if (existingStudent != null)
            {
                existingStudent.FirstName=request.FirstName;
                existingStudent.LastName=request.LastName;
                existingStudent.DateofBirth=request.DateofBirth;
                existingStudent.Email=request.Email;
                existingStudent.Mobile=request.Mobile;
                existingStudent.GenderId=request.GenderId;
                existingStudent.Address.PhysicalAddress=request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress=request.Address.PostalAddress;

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            else
            {
                return null;
            }

             
        }

        public async  Task<Student> DeleteStudent(Guid studentId)
        {
            var student=await GetStudentAsync(studentId);

            if (student != null)
            {
                _context.Student.Remove(student);
               await _context.SaveChangesAsync();
               return student;
            }

            return null;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            var newStudent= await _context.Student.AddAsync(student);
           await _context.SaveChangesAsync();
           return newStudent.Entity;
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
