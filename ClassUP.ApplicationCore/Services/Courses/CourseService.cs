using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Thumbnail;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IThumbnailService _thumbnailService;
        public CourseService(IUnitOfWork unitOfWork , IThumbnailService thumbnailService)
        {
            _unitOfWork = unitOfWork;
            _thumbnailService = thumbnailService;
        }



        #region Create
        public async Task<CreateCourseDTO> CreateCourse(CreateCourseRequest request, int userId)
        {
            if (request.Thumbnail == null)
                throw new ArgumentException("Thumbnail is required");

            if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
                throw new ArgumentException("Invalid course level");

            var thumbnailDTO = new ThumbnailDTO
            {
                FileStream = request.Thumbnail.OpenReadStream(),
                MimeType = request.Thumbnail.ContentType,
                FileSize = request.Thumbnail.Length
            };

            var thumbnailUrl = await _thumbnailService.SaveAsync(thumbnailDTO, "courses");

            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Level = level,
                Language = request.Language,
                IsActive = request.IsActive,
                UserId = userId,
                ThumbnailUrl = thumbnailUrl,
                CategoryId = request.CategoryId
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return new CreateCourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Level = course.Level.ToString(),
                Price = course.Price,
                Language = course.Language,
                ThumbnailUrl = course.ThumbnailUrl
            };
        }



        #endregion

        #region Delete

        // search how implement Soft Delete 
        public async Task DeleteCourse(int courseId)
         {
             var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
             if (course == null)
             {

               throw new KeyNotFoundException($"Course with id {courseId} not found");
             }
             await _unitOfWork.Courses.DeleteAsync(course);
             await _thumbnailService.DeleteAsync(course.ThumbnailUrl);
             await _unitOfWork.SaveChangesAsync();

         } 
        #endregion

        #region GetAll
        public async Task<IEnumerable<AllCoursesDTO>> GetAllCourses(FilterOptions filter)
        {
            var courses = await _unitOfWork.Courses.GetAllAsync(filter);

            if (courses == null || !courses.Any())
                return Enumerable.Empty<AllCoursesDTO>();

            return courses.Select(MapToAllCoursesDto);
        }

        #endregion

        public async Task<IEnumerable<AllCoursesDTO>> GetCategoryCourses(int categoryId)
        {
            var courses = await _unitOfWork.Courses.GetCategoryCoursesAsync(categoryId);

            if (courses == null || !courses.Any())
                return Enumerable.Empty<AllCoursesDTO>();

            return courses.Select(MapToAllCoursesDto);
        }

        #region GetById
        public async Task<CourseDetailsDTO> GetByIdAsync(int id)
         {
             var Course= await _unitOfWork.Courses.GetByIdAsync(id);
            if (Course == null)
                throw new KeyNotFoundException($"Course with id {id} not found");


            return MapToCourseDetailsDto(Course); 
         }

       

        #endregion



        #region GetCorsesByInstractor
        public async Task<IEnumerable<AllCoursesDTO>> GetInstructorCoursesAsync(int instructorId, FilterOptions filter)
         {
            var courses = await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);

            if (courses == null || !courses.Any())
                return Enumerable.Empty<AllCoursesDTO>();

            return courses.Select(MapToAllCoursesDto);

        }

        #endregion

        #region UpdateCourse
         public async Task UpdateCourse(int userId, UpdateCourseRequest request)
         {
             if (request == null )
                 throw new ArgumentNullException(nameof(request));

             var course = await _unitOfWork.Courses.GetByIdAsync(request.courseId);
             if (course == null)
                 throw new KeyNotFoundException($"Course with ID {request.courseId} not found");

             // Partial Update
             course.Title = string.IsNullOrWhiteSpace(request.Title) ? course.Title : request.Title;
             course.Description = string.IsNullOrWhiteSpace(request.Description) ? course.Description : request.Description;
             course.Price = request.Price ?? course.Price;
             course.Language = string.IsNullOrWhiteSpace(request.Language) ? course.Language : request.Language;
             course.IsActive = request.IsActive ?? course.IsActive;

             // Enum 
             if (!string.IsNullOrWhiteSpace(request.Level))
             {
                 if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
                     throw new ArgumentException("Invalid course level");

                 course.Level = level;
             }

             // Thumb
             if (request.Thumbnail != null)
            {
                 var oldThumbnail = course.ThumbnailUrl;
                 var thumbnailDTO = new ThumbnailDTO
                 {
                    FileStream = request.Thumbnail.OpenReadStream(),
                    MimeType = request.Thumbnail.ContentType,
                    FileSize = request.Thumbnail.Length
                 };

                 course.ThumbnailUrl = await _thumbnailService.SaveAsync(thumbnailDTO, "courses");

                 if (!string.IsNullOrWhiteSpace(oldThumbnail) && oldThumbnail != course.ThumbnailUrl)
                 {
                     await _thumbnailService.DeleteAsync(oldThumbnail);
                 }
             }

             await _unitOfWork.Courses.UpdateAsync(course);
             await _unitOfWork.SaveChangesAsync();
         }

         
        #endregion


        #region Mapping Methods
        private AllCoursesDTO MapToAllCoursesDto(Course course) =>
       new AllCoursesDTO
       {
           Id = course.Id,
           Title = course.Title,
           Price = course.Price,
           Level = course.Level,
           Language = course.Language,
           IsActive = course.IsActive,
           InstructorId = course.UserId,
           ThumbnailUrl = course.ThumbnailUrl,
           CategoryId = course.CategoryId,
       };

        private CourseDetailsDTO MapToCourseDetailsDto(Course course)
        {
            return new CourseDetailsDTO
            {
                // Main Properties
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Level = course.Level,
                Price = course.Price,
                ThumbnailUrl = course.ThumbnailUrl,
                PreviewVideoUrl = course.PreviewVideoUrl,
                IsPublished = course.IsPublished,
                PublishedAt = course.PublishedAt,

                // Instructor Info
                InstructorId = course.UserId,

                // Category Info
                CategoryId = course.CategoryId,
                CategoryName = course.Category?.Name ?? string.Empty,

                // Statistics
                TotalSections = course.Sections?.Count ?? 0,
                TotalEnrollments = course.Enrollments?.Count ?? 0,
                TotalReviews = course.Reviews?.Count ?? 0,
                AverageRating = (course.Reviews != null && course.Reviews.Any())
                    ? Math.Round(course.Reviews.Average(r => r.Rating), 2)
                    : 0
            };
        }

        #endregion




    }

}

