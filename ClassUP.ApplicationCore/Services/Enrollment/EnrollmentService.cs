using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollments;
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
        public async Task<IEnumerable<EnrollmentDTO>> GetAllAsync()
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetAllAsync(null);

            return enrollments.Items.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,
                StudentId = e.UserId,
                EnrolledAt = e.EnrolledAt,
                ProgressPercentage = e.ProgressPercentage,
                CompletedAt = e.CompletedAt,

            });
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

        public async Task<IEnumerable<EnrollmentDTO>> GetStudentEnrollmentsAsync(string userId)
        {
            var allEnrollments = await _unitOfWork.Enrollments.GetAllAsync(null);

            var enrollments = allEnrollments.Items
                .Where(e => e.UserId == userId)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.Id,
                    CourseId = e.CourseId,
                    StudentId = e.UserId,
                    EnrolledAt = e.EnrolledAt,
                    ProgressPercentage = e.ProgressPercentage,
                    CompletedAt = e.CompletedAt,
                });

            return enrollments;
        }

        public async Task<CheckEnrollmentResponse> IsEnrolledAsync(int courseId, string userId)
        {
            if (courseId <= 0)
                return new CheckEnrollmentResponse
                {
                    IsEnrolled = false,
                    EnrollmentDate = null
                };

            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, courseId);

            if (enrollment == null)
            {
                return new CheckEnrollmentResponse
                {
                    IsEnrolled = false,
                    EnrollmentDate = null
                };
            }

            return new CheckEnrollmentResponse
            {
                IsEnrolled = true,
                EnrollmentDate = enrollment.EnrolledAt
            };
        }

        public async Task<EnrollmentDTO> CreateAsync(int CourseId, string UserId)
        {
            if (CourseId <= 0)
                return null;

            var alreadyEnrolled = await _unitOfWork.Enrollments
               .IsEnrolledAsync(UserId, CourseId);
            if (alreadyEnrolled)
                return null;
            var course = await _unitOfWork.Courses
               .GetByIdAsync(CourseId);
            if (course == null)
                return null;
            var enrollment = new Domain.Models.Enrollment
            {
                CourseId = CourseId,
                UserId = UserId,
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
        public async Task UnEnrollAsync(int courseId, string userId)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, courseId);

            if (enrollment == null)
                return;

            _unitOfWork.Enrollments.DeleteAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

