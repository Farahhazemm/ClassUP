using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.LectursProgress
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProgressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task MarkLessonAsCompletedAsync(int lectureId, string userId)
        {

            var lecture = await _unitOfWork.Lectures.GetLectureWithSectionAndCourseAsync(lectureId);
            if (lecture == null)
                throw new Exception("Lecture not found");

            var enrollment = await _unitOfWork.Enrollments
                 .GetEnrollmentAsync(userId, lecture.Section.CourseId);
            if (enrollment == null)
                throw new Exception("User is not enrolled in this course");
            var progress = await _unitOfWork.Progresses .GetByEnrollmentAndLectureAsync(enrollment.Id, lectureId);
            if (progress != null)
            {
                // already exists => mark completed if not
                if (!progress.IsCompleted)
                {
                    progress.IsCompleted = true;
                    progress.CompletedAt = DateTime.UtcNow;
                   
                }

                await _unitOfWork.SaveChangesAsync();
                return;
            }
            //  Create new progress
            var lectureProgress = new LectureProgress
            {
                EnrollmentId = enrollment.Id,
                LectureId = lectureId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };

            await _unitOfWork.Progresses.AddAsync(lectureProgress);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UnCompleteLessonAsync(int lectureId, string userId)
        {
            if (lectureId <= 0)
                throw new ArgumentException("Invalid lesson ID");

           
            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, lectureId);

            if (enrollment == null)
                throw new Exception("User is not enrolled in this course");

            
            var progress = await _unitOfWork.Progresses
                .GetByEnrollmentAndLectureAsync(enrollment.Id, lectureId);

            if (progress == null)
                throw new Exception("Progress not found");

            
            progress.IsCompleted = false;
            progress.CompletedAt = null;

            await _unitOfWork.SaveChangesAsync();
        }



    }

}
