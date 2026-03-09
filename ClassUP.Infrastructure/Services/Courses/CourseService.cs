using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Courses;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
using ClassUP.ApplicationCore.Exeptions;
using ClassUP.ApplicationCore.Helpers.Filters;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.IImage;
using ClassUP.Domain.Constants;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageValidator _imageValidator;
        private readonly ICloudinaryService _cloudinaryService;
        public CourseService(IUnitOfWork unitOfWork  , IImageValidator imageValidator , ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
           _cloudinaryService = cloudinaryService;
            _imageValidator = imageValidator;
        }



        #region Create
        public async Task<CreateCourseDTO> CreateCourse(CreateCourseRequest request, string userId)
        {
            if (request == null)
                throw new BadRequestException("Request body is required");

            _imageValidator.Validate(request.Thumbnail);
            var uploadResult = await _cloudinaryService.UploadAsync(request.Thumbnail, $"courses/{userId}");
            var thumbnailUrl = uploadResult.Url;
            var thumbnailPublicId = uploadResult.PublicId;

            if (!Enum.TryParse<CourseLevel>(request.Level, true, out var level))
                throw new BadRequestException("Invalid course level");

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
                ThumbnailPublicId = thumbnailPublicId, 
                CategoryId = request.CategoryId
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return new CreateCourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Level = course.Level.ToString(),
                Description= course.Description,
                Price = course.Price,
                Language = course.Language,
                ThumbnailUrl = course.ThumbnailUrl,
                IsActive= course.IsActive,
                CategoryId = course.CategoryId

            };
        }



        #endregion

        #region Delete

        // search how implement Soft Delete 
        public async Task DeleteCourse(int courseId, string userId, bool isAdmin)
        {
           
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course", courseId);


            if (course.UserId != userId && !isAdmin)
                throw new BadRequestException("You are not allowed to delete this course");


            await _unitOfWork.Courses.DeleteAsync(course);
            if (!string.IsNullOrWhiteSpace(course.ThumbnailPublicId))
            {
                await _cloudinaryService.DeleteAsync(course.ThumbnailPublicId);
            }


            await _unitOfWork.SaveChangesAsync();
        }


        #endregion

        #region GetAll
        public async Task<PaginatedList<AllCoursesDTO>> GetAllCourses(FilterOptions filter)
        {
            var courses = await _unitOfWork.Courses.GetAllAsync(filter); 

            if (courses == null || !courses.Items.Any())
                return new PaginatedList<AllCoursesDTO>(new List<AllCoursesDTO>(), 1, 0, filter.PageSize);

         
            var courseDtos = courses.Items.Select(MapToAllCoursesDto).ToList();

         
            return new PaginatedList<AllCoursesDTO>(
                courseDtos,
                courses.PageNumber,
                courses.TotalPages * courses.Items.Count, 
                courseDtos.Count
            );
        }

        #endregion


        #region GetByCategory
        public async Task<IEnumerable<AllCoursesDTO>> GetCategoryCourses(int categoryId)
        {
            var courses = await _unitOfWork.Courses.GetCategoryCoursesAsync(categoryId);

            if (courses == null || !courses.Any())
                throw new NotFoundException("category", categoryId);

            return courses.Select(MapToAllCoursesDto);
        } 
        #endregion

        #region GetById
        public async Task<CourseDetailsDTO> GetByIdAsync(int id)
         {
             var Course= await _unitOfWork.Courses.GetCourseDetailsAsync(id);
            if (Course == null)
                throw new NotFoundException("Course", id);


            return MapToCourseDetailsDto(Course); 
         }

       

        #endregion



        #region GetCorsesByInstractor
        public async Task<IEnumerable<AllCoursesDTO>> GetInstructorCoursesAsync(string instructorId, FilterOptions filter)
         {
            var courses = await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);

            if (courses == null || !courses.Any())
                return Enumerable.Empty<AllCoursesDTO>();

            return courses.Select(MapToAllCoursesDto);

        }

        #endregion


        #region UpdateCourse
        public async Task UpdateCourse(string userId, bool isAdmin, UpdateCourseRequest request)
        {
            if (request == null)
                throw new BadRequestException("Request body is required");

            var course = await _unitOfWork.Courses.GetByIdAsync(request.courseId);
            if (course == null)
                throw new NotFoundException("Course", request.courseId);
            //  Auth check
            if (course.UserId != userId && !isAdmin)
                throw new BadRequestException("You are not allowed to update this course");

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
                    throw new BadRequestException("Invalid course level");
                course.Level = level;
            }

            // Thumb
            if (request.Thumbnail != null)
            {
                var oldPublicId = course.ThumbnailPublicId;

                _imageValidator.Validate(request.Thumbnail);
                var uploadResult = await _cloudinaryService.UploadAsync(request.Thumbnail, $"courses/{course.UserId}");

                course.ThumbnailUrl = uploadResult.Url;
                course.ThumbnailPublicId = uploadResult.PublicId;

                if (!string.IsNullOrWhiteSpace(oldPublicId) && oldPublicId != course.ThumbnailPublicId)
                {
                    await _cloudinaryService.DeleteAsync(oldPublicId);
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

