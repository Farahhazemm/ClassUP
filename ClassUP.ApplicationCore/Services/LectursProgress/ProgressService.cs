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

        public async Task<bool> IsLessonCompletedAsync(int lectureId, string userId)
        {
            
            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, lectureId); 

            if (enrollment == null)
                throw new Exception("User is not enrolled in this course");

           
            var progress = await _unitOfWork.Progresses
                .GetByEnrollmentAndLectureAsync(enrollment.Id, lectureId);

            if (progress == null)
                return false;

            return progress.IsCompleted;
        }


        public async Task<IEnumerable<int>> GetCompletedLessonsAsync(int courseId, string userId)
        {
            
            var enrollment = await _unitOfWork.Enrollments
                .GetEnrollmentAsync(userId, courseId);

            if (enrollment == null)
                throw new Exception("User is not enrolled in this course");

            
            var completedLessons = await _unitOfWork.Progresses
                .GetAllAsync(null); 

            return completedLessons
                .Where(lp => lp.EnrollmentId == enrollment.Id && lp.IsCompleted)
                .Select(lp => lp.LectureId);
        }

        public async Task<float> RecalculateProgressAsync(int enrollmentId)
        {
            // جلب ال enrollment
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(enrollmentId);
            if (enrollment == null)
                throw new Exception("Enrollment not found");

         
            var totalLectures = await _unitOfWork.Lectures.GetAllAsync(null);

            
            int totalCount = totalLectures.Count(l => l.Section != null && l.Section.CourseId == enrollment.CourseId);

            if (totalCount == 0)
                return 0;

            
            var allProgress = await _unitOfWork.Progresses.GetAllAsync(null);

            
            int completedLessons = allProgress.Count(lp =>
                lp.EnrollmentId == enrollmentId &&
                lp.IsCompleted &&
                lp.lecture != null &&
                lp.lecture.Section != null &&
                lp.lecture.Section.CourseId == enrollment.CourseId);

            
            float progressPercentage = totalCount > 0
                ? ((float)completedLessons / totalCount) * 100
                : 0;

            progressPercentage = (float)Math.Round(progressPercentage);

            
            enrollment.ProgressPercentage = progressPercentage;

           
            if (Math.Abs(progressPercentage - 100) < 0.01)
            {
                enrollment.Status = "Completed";
                enrollment.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                enrollment.Status = "Active";
                enrollment.CompletedAt = null;
            }

            
            await _unitOfWork.SaveChangesAsync();

            return progressPercentage;
        }




    }

}
