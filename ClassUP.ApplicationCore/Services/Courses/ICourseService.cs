using ClassUP.ApplicationCore.Common.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using ClassUP.Domain.Models;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;
namespace ClassUP.ApplicationCore.Services.Courses

{
    public interface ICourseService
    {
        Task<IEnumerable<AllCoursesDTO>> GetAllCourses(FilterOptions filter); // made for test
        Task<IEnumerable<AllCoursesDTO>> GetInstructorCoursesAsync(string instructorId,FilterOptions filter );
        Task<IEnumerable<AllCoursesDTO>> GetCategoryCourses(int categoryId);
       Task<CourseDetailsDTO> GetByIdAsync(int id);
       Task<CreateCourseDTO> CreateCourse(CreateCourseRequest request, string userId);
        Task UpdateCourse(string userId, UpdateCourseRequest request);

        Task DeleteCourse(int courseId);
    }
}
