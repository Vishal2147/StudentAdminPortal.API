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


    }
}
