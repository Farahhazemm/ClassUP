using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollments;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Enrollment
{
    public interface IEnrollmentService
    {
        //Task<EnrollmentDTO> CreateAsync(int CourseId, string UserId);
        Task<EnrollmentDTO> GetByIdAsync(int id);
        Task<PaginatedList<EnrollmentDTO>> GetStudentEnrollmentsAsync(string userId, FilterOptions filter);
        Task<PaginatedList<EnrollmentDTO>> GetAllAsync(FilterOptions filter);
        Task<CheckEnrollmentResponse> IsEnrolledAsync(int courseId, string userId);
        Task UnEnrollAsync(int courseId, string userId);


    }
}
