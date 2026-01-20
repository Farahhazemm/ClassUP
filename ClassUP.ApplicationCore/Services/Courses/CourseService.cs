using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Cources;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Thumbnail;
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

        public Task DeleteCourse(int courseId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Course>> GetAllCourses(FilterOptions filter)
        {
            var Courses = await _unitOfWork.Courses.GetAllAsync(filter);
            return  Courses.ToList(); 
        }

        public async Task<Course> GetByIdAsync(int id)
        {
           return await _unitOfWork.Courses.GetByIdAsync(id);
        }

       

        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId, FilterOptions filter)
        {
            return await _unitOfWork.Courses.GetInstructorCoursesAsync(instructorId, filter);
        }

        public async Task UpdateCourse(int courseId, UpdateCourseDTO courseDTO, ThumbnailDTO? thumbnailDTO, int userId)
        {
            if (courseDTO == null && thumbnailDTO == null)
                return; //or make badrequest


            //get old course
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
            {
                //Make an Exeption
            }
            string? oldThumbnail = course.ThumbnailUrl;
            if (thumbnailDTO != null)
            {
                //validate Thumbnail not for now

                course.ThumbnailUrl = await _thumbnailService
                    .SaveAsync(thumbnailDTO, "courses");
            }
                // Op updates
                course.Title = courseDTO.Title ?? course.Title;
                course.Description = courseDTO.Description ?? course.Description;
                course.Price = courseDTO.Price ?? course.Price;
                course.Level = courseDTO.Level ?? course.Level;
                course.Language = courseDTO.Language ?? course.Language;
                course.IsActive = courseDTO.IsActive ?? course.IsActive;

                await _unitOfWork.Courses.UpdateAsync(course);

                // Del old thumbnail if replace
                if (oldThumbnail != course.ThumbnailUrl && oldThumbnail != null)
                {
                    await _thumbnailService.DeleteAsync(oldThumbnail);
                }

               
                await _unitOfWork.SaveChangesAsync();

            }

        public Task UpdateCourse(int courseId, UpdateCourseDTO courseDTO, int userId, ThumbnailDTO? thumbnailDTO = null)
        {
            throw new NotImplementedException();
        }
    }

    }

