using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Cources;
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
        public async Task<CourseResponseDTO> CreateCourse(CreateCourseDTO courseDTO, ThumbnailDTO thumbnailDTO, int userId)
        {
            var thumbnailUrl = await _thumbnailService.SaveAsync(thumbnailDTO, "courses");


            var course = new Course
            {
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                Price = courseDTO.Price,
                Level = courseDTO.Level,
                Language = courseDTO.Language,
                IsActive = courseDTO.IsActive,
                InstructorId = userId,
                ThumbnailUrl = thumbnailUrl,

            };
            await _unitOfWork.Courses.AddAsync(course);

            await _unitOfWork.SaveChangesAsync();
            var response = new CourseResponseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Level = course.Level,
                Language = course.Language,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                ThumbnailUrl = course.ThumbnailUrl,

            };
            return response;

        }

        #endregion

        #region Delete
        public async Task DeleteCourse(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                // Run an Exeption
                throw new KeyNotFoundException($"Course with id {courseId} not found");
            }
            await _unitOfWork.Courses.DeleteAsync(course);
            await _thumbnailService.DeleteAsync(course.ThumbnailUrl);
            await _unitOfWork.SaveChangesAsync();

        } 
        #endregion

        #region GetAll
        public async Task<IEnumerable<Course>> GetAllCourses(FilterOptions filter)
        {
            var Courses = await _unitOfWork.Courses.GetAllAsync(filter);
            return Courses.ToList();
        }
        #endregion

        #region GetById
        public async Task<Course> GetByIdAsync(int id)
        {
            return await _unitOfWork.Courses.GetByIdAsync(id);
        }
        #endregion



        #region GetCorsesByInstractor
        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId, FilterOptions filter)
        {
            return await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);
        }
        #endregion

        #region UpdateCourse
        public async Task UpdateCourse(int courseId, UpdateCourseDTO courseDTO, int userId, ThumbnailDTO? thumbnailDTO)
        {
            if (courseDTO == null && thumbnailDTO == null)
                throw new ArgumentException("Nothing to update");

            
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with id {courseId} not found");


            if (courseDTO.Level != null)
            {
                if (!Enum.TryParse<CourseLevel>(courseDTO.Level, true, out var level))
                    throw new ArgumentException("Invalid course level");

                course.Level = level;
            }

            // Op update
            course.Title = courseDTO.Title ?? course.Title;
            course.Description = courseDTO.Description ?? course.Description;
            course.Price = courseDTO.Price ?? course.Price;
            course.Language = courseDTO.Language ?? course.Language;
            course.IsActive = courseDTO.IsActive ?? course.IsActive;

            // Thumb
            if (thumbnailDTO != null)
            {
                var oldThumbnail = course.ThumbnailUrl;
                course.ThumbnailUrl = await _thumbnailService.SaveAsync(thumbnailDTO, "courses");
                if (oldThumbnail != null && oldThumbnail != course.ThumbnailUrl)
                    await _thumbnailService.DeleteAsync(oldThumbnail);
            }

            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }



        #endregion

    }

}

