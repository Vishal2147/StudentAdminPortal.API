using System;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Reopsitories;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace StudentAdminPortal.API.Controllers
{

    [ApiController]
    
    public class StudentController : Controller
    {
        internal readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)      // need to inject studentRepository
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }                    


        [HttpGet]
        [Route("[controller]")]
        public async  Task<IActionResult> GetAllStudents()
        {
            /*return Ok(_studentRepository.GetStudents());*/
            // return from domain model now instead of datamodel

            var students = await  _studentRepository.GetStudentsAsync();
            return Ok(_mapper.Map<List<Student>>(students));       // list<student> is domain model student

            /*Without AutoMapper
             
            var domainModelStudents = new List<Student>();

            foreach (var student in students)
            {
                domainModelStudents.Add(new Student()
                {
                    Id=student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateofBirth = student.DateofBirth,
                    Email = student.Email,
                    Mobile = student.Mobile,
                    ProfileImageUrl = student.ProfileImageUrl,
                    GenderId = student.GenderId,
                    Gender = new Gender()            // to differentiate b/w model and domain objecy
                    {
                         Id = student.Gender.Id,
                         Description = student.Gender.Description
                    },
                    Address = new Address()
                    {
                        Id = student.Address.Id,
                        PhysicalAddress = student.Address.PhysicalAddress,
                        PostalAddress = student.Address.PostalAddress
                    }
                    
                });
            }*/



        }


        [HttpGet]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // fetch student
            var student = await _studentRepository.GetStudentAsync(studentId);


            //return student

            if (student == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Student>(student));    // map domain model to student model
        }

    }
}
