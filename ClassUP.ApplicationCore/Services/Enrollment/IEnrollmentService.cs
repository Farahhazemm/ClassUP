using ClassUP.ApplicationCore.DTOs.Requests.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
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
    }
}
