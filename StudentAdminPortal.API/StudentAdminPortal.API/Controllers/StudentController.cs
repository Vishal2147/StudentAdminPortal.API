using System;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Reopsitories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace StudentAdminPortal.API.Controllers
{

    [ApiController]

    public class StudentController : Controller
    {
        internal readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IImageRespository _imageRepository;

        public StudentController(IStudentRepository studentRepository,
            IMapper mapper,IImageRespository imageRepository) // need to inject studentRepository
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }


        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            /*return Ok(_studentRepository.GetStudents());*/
            // return from domain model now instead of datamodel

            var students = await _studentRepository.GetStudentsAsync();
            return Ok(_mapper.Map<List<Student>>(students)); // list<student> is domain model student

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
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]

        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // fetch student
            var student = await _studentRepository.GetStudentAsync(studentId);


            //return student

            if (student == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Student>(student)); // map domain model to student model
        }



        //put Update API

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId,
            [FromBody] UpdateStudentRequest request)
        {

            if (await _studentRepository.Exists(studentId))
            {
                //update details

                var updatedStudent =
                    await _studentRepository.UpdateStudentMethod(studentId,
                        _mapper.Map<DataModels.Student>(request)); // map request to data models student

                if (updatedStudent != null)
                {
                    return Ok(_mapper.Map<DomainModels.Student>(updatedStudent));
                }
            }


            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.Exists(studentId))
            {
                var student = await _studentRepository.DeleteStudent(studentId);

                return Ok(_mapper.Map<Student>(student));
            }

            return NotFound();
        }


        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var newStudent = await _studentRepository.AddStudentAsync(_mapper.Map<DataModels.Student>(request));

            /*return Ok(newStudent);*/

            return CreatedAtAction(nameof(GetStudentAsync), new {studentId = newStudent.Id},
                _mapper.Map<Student>(newStudent));

        }

          
        //Image Upload api

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
                ".jpeg",
                ".png",
                "gif",
                ".jpg"
            };

            if (profileImage != null && profileImage.Length>0)
            {

                var extension = Path.GetExtension(profileImage.FileName);

                if (validExtensions.Contains(extension))
                {
                    //check if student exists
                    if (await _studentRepository.Exists(studentId))
                    {
                        //upload the image to local storage
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await _imageRepository.Upload(profileImage, fileName);


                        //update the profile image path in the database

                        if (await _studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }

                }
            }
                


            return BadRequest("This is not valid Extension");
        }
    }



}

