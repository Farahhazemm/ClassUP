using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollment;
using ClassUP.ApplicationCore.DTOs.Responses.Enrollments;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Helpers.Filters;
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
        public async Task<PaginatedList<EnrollmentDTO>> GetAllAsync(FilterOptions filter)
        {
          
            filter.PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            filter.PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

           
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync(filter);

           
            if (enrollments == null || !enrollments.Items.Any())
                return new PaginatedList<EnrollmentDTO>(new List<EnrollmentDTO>(), 0, filter.PageNumber, filter.PageSize);

           
            var dtoList = enrollments.Items.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,
                StudentId = e.UserId,
                EnrolledAt = e.EnrolledAt,
                ProgressPercentage = e.ProgressPercentage,
                CompletedAt = e.CompletedAt
            }).ToList();

           
            return new PaginatedList<EnrollmentDTO>(
                dtoList,
                enrollments.TotalCount, 
                enrollments.PageNumber,
                enrollments.PageSize
            );
        }
        public async Task<EnrollmentDTO> GetByIdAsync(int id)
        {
            if (id <= 0)
               throw new BadRequestException ("This Id Is Not Valid");

            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);

            if (enrollment == null)
                throw new NotFoundException("Enrollment");  

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

        public async Task<PaginatedList<EnrollmentDTO>> GetStudentEnrollmentsAsync(string userId, FilterOptions filter)
        {
            var enrollments = await _unitOfWork.Enrollments
                .GetStudentEnrollmentsAsync(userId, filter);

            var dtoList = enrollments.Items.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,
                StudentId = e.UserId,
                EnrolledAt = e.EnrolledAt,
                ProgressPercentage = e.ProgressPercentage,
                CompletedAt = e.CompletedAt,
            }).ToList();

            return new PaginatedList<EnrollmentDTO>(
                dtoList,
                enrollments.TotalCount,
                enrollments.PageNumber,
                enrollments.PageSize
            );
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

        //public async Task<EnrollmentDTO> CreateAsync(int CourseId, string UserId)
        //{
        //    if (CourseId <= 0)
        //        throw new BadRequestException("Invalid course id");

        //    var alreadyEnrolled = await _unitOfWork.Enrollments
        //       .IsEnrolledAsync(UserId, CourseId);
        //    if (alreadyEnrolled)
        //        throw new BadRequestException("Already enrolled");
        //    var course = await _unitOfWork.Courses
        //       .GetByIdAsync(CourseId);
        //    if (course == null)
        //        throw new NotFoundException("Course");
        //    var enrollment = new Domain.Models.Enrollment
        //    {
        //        CourseId = CourseId,
        //        UserId = UserId,
        //        EnrolledAt = DateTime.UtcNow,
        //        ProgressPercentage = 0,
        //        CompletedAt = null,

        //    };

        //    await _unitOfWork.Enrollments.AddAsync(enrollment);
        //    await _unitOfWork.SaveChangesAsync();
        //    return new EnrollmentDTO
        //    {
        //        EnrollmentId = enrollment.Id,
        //        CourseId = enrollment.CourseId,
        //        StudentId = enrollment.UserId,
        //        EnrolledAt = enrollment.EnrolledAt,
        //        ProgressPercentage = enrollment.ProgressPercentage,
        //        CompletedAt = enrollment.CompletedAt,

        //    };
        //}
        public async Task UnEnrollAsync(int courseId, string userId)
        {
            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, courseId);

            if (enrollment == null)
                throw new NotFoundException("Enrollment");

            await _unitOfWork.Enrollments.DeleteAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

