using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Cources;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Thumbnail;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IThumbnailService _thumbnailService;
        public CourseService(IUnitOfWork unitOfWork , IThumbnailService thumbnailService)
        {
            _unitOfWork = unitOfWork;
            _thumbnailService = thumbnailService;
        }

        public async Task<CourseResponseDTO> CreateCourse(CreateCourseDTO courseDTO, ThumbnailDTO thumbnailDTO, int userId)
        {
            var thumbnailUrl = await _thumbnailService.SaveAsync(thumbnailDTO, "courses");


            var course = new Course
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                Price = courseDTO.Price,
                Level = courseDTO.Level,
                Language = courseDTO.Language,
                IsActive = courseDTO.IsActive,
                InstructorId = userId,
                ThumbnailUrl = thumbnailUrl,

            };
            await _unitOfWork.Courses.AddAsync(course);

            await _unitOfWork.SaveChangesAsync();
            var response = new CourseResponseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Level = course.Level,
                Language = course.Language,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                ThumbnailUrl = course.ThumbnailUrl,
                
            };
            return response;

        }

        public async Task<IEnumerable<Course>> GetAllCourses(FilterOptions filter)
        {
            var Courses = await _unitOfWork.Courses.GetAllAsync(filter);
            return  Courses.ToList(); 
        }

        public async Task<Course> GetByIdAsync(int id)
        {
           return await _unitOfWork.Courses.GetByIdAsync(id);
        }

       

        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId, FilterOptions filter)
        {
            return await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);
        }

       
    }
}
