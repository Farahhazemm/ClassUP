using ClassUP.ApplicationCore.DTOs.Requests.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Enrollment
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EnrollmentDTO> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);

            if (enrollment == null)
                return null;

            return new EnrollmentDTO
            {
                EnrollmentId = enrollment.Id,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.UserId,
                EnrolledAt = enrollment.EnrolledAt,
                ProgressPercentage = enrollment.ProgressPercentage,
                CompletedAt = enrollment.CompletedAt,
                
            };
        }

        public async Task<EnrollmentDTO> CreateAsync(CreateEnrollmentRequest request)
        {
            if (request.CourseId <= 0 || request.StudentId <= 0)
                return null;

           /* var alreadyEnrolled = await _unitOfWork.Enrollments
               .IsEnrolledAsync(request.StudentId, request.CourseId);
            if (alreadyEnrolled)
                return null;*/
            var course = await _unitOfWork.Courses
               .GetByIdAsync(request.CourseId);
            if (course == null)
                return null;
            var enrollment = new Domain.Models.Enrollment
            {
                CourseId = request.CourseId,
                UserId = request.StudentId,
                EnrolledAt = DateTime.Now,
                ProgressPercentage = 0,
                CompletedAt = null,
              
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return new EnrollmentDTO
            {
                EnrollmentId = enrollment.Id,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.UserId,
                EnrolledAt = enrollment.EnrolledAt,
                ProgressPercentage = enrollment.ProgressPercentage,
                CompletedAt = enrollment.CompletedAt,
               
            };
        }
    }
}
