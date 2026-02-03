using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Lectures
{
    public class LectureService : ILectureService
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public LectureService( IUnitOfWork unitOfWork)
        {
           
            _unitOfWork = unitOfWork;
        }

       

        #region GetAll
        public async Task<IEnumerable<LectureDto>> GetLecturesAsync(FilterOptions filterOptions)
        {
            var lectures = await _unitOfWork.Lectures.GetAllAsync(filterOptions);
            if (lectures == null || !lectures.Any())
                return Enumerable.Empty<LectureDto>();

            return lectures.Select(MapToLectureDto);

        }
        #endregion

        #region GetById
        public async Task<LectureDetailDto?> GetByIdAsync(int id)
        {

            var lecture = await _unitOfWork.Lectures.GetByIdWithDetailsAsync(id);
            if (lecture == null)
                throw new KeyNotFoundException("Lecture not found");
            return new LectureDetailDto
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                Type = lecture.Type,
                Duration = lecture.Duration,
                OrderIndex = lecture.OrderIndex,
                SectionId = lecture.SectionId,
                IsFree = lecture.IsFree,

                VideoContent = lecture.Type == "Video" && lecture.VideoContent != null
    ? new VideoContentDTO
    {
        Id = lecture.VideoContent.Id,
        VideoUrl = lecture.VideoContent.VideoUrl,

    }
    : null,


                ArticleContent = lecture.Type == "Article" && lecture.ArticleContent != null
    ? new ArticleContentDTO
    {
        Id = lecture.ArticleContent.Id,
        ArticleText = lecture.ArticleContent.Content
    }
    : null,


                LectureProgresses = lecture.LectureProgresses
                .Select(p => new LectureProgressDTO
                {
                    Id = p.Id,
                    IsCompleted = p.IsCompleted,
                    WatchedDuration = p.WatchedDuration,
                    LastWatchedAt = p.LastWatchedAt,
                    CompletedAt = p.CompletedAt
                })
                .ToList()
            };
        } 
        #endregion

        #region MapMethod
        private LectureDto MapToLectureDto(Lecture lecture)
        {
            return new LectureDto
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                Type = lecture.Type,
                Duration = lecture.Duration,
                OrderIndex = lecture.OrderIndex,
                SectionId = lecture.SectionId,
                IsFree = lecture.IsFree
            };
        }


        #endregion

        public async Task AddAsync(int userId, CreateLectureRequest request)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(request.SectionId);
            if (section == null)
                throw new ArgumentException("Section not found");

            if (request.Type != "video" && request.Type != "article")
                throw new ValidationException("Lecture type must be Video or Article");

            if (request.Type == "Video" && string.IsNullOrWhiteSpace(request.VideoUrl))
                throw new ValidationException("VideoUrl is required for Video lectures");

            if (request.Type == "Article" && string.IsNullOrWhiteSpace(request.ArticleContent))
                throw new ValidationException("ArticleContent is required for Article lectures");

          //  var orderIndex = await _unitOfWork.Lectures.CountAsync(l => l.SectionId == request.SectionId) + 1;

            var lecture = new Lecture
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                Duration = request.Duration ?? 0,
                IsFree = request.IsFree,
                SectionId = request.SectionId,
               // OrderIndex = orderIndex,

                VideoContent = request.Type == "Video"
            ? new VideoContent
            {
                VideoUrl = request.VideoUrl!
            }
            : null,

                ArticleContent = request.Type == "Article"
            ? new ArticleContent
            {
                Content = request.ArticleContent!
            }
            : null
            };

            
            await _unitOfWork.Lectures.AddAsync(lecture);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    }
