using ClassUP.ApplicationCore.DTOs.Requests.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollments;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Enrollment
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDTO> CreateAsync(CreateEnrollmentRequest request);
        Task<EnrollmentDTO> GetByIdAsync(int id);
        Task<IEnumerable<EnrollmentDTO>> GetStudentEnrollmentsAsync(string userId);
        Task<IEnumerable<EnrollmentDTO>> GetAllAsync();
        Task<CheckEnrollmentResponse> IsEnrolledAsync(int courseId, string userId);
        Task UnEnrollAsync(int courseId, string userId);

    }
}
