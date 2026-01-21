using ClassUP.ApplicationCore.Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using ClassUP.Domain.Models;
using ClassUP.ApplicationCore.DTOs.Cources;
namespace ClassUP.ApplicationCore.Services.Courses

{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCourses(FilterOptions filter); // made for test
        Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId,FilterOptions filter );
        Task<Course> GetByIdAsync(int id);
        Task<CourseResponseDTO> CreateCourse(CreateCourseDTO courseDTO, ThumbnailDTO thumbnailDTO, int userId);
        Task UpdateCourse(int courseId, UpdateCourseDTO courseDTO ,int userId, ThumbnailDTO? thumbnailDTO=null);

        Task DeleteCourse(int courseId);
    }
}
