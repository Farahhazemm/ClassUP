using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
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
        ThumbnailUrl = lecture.VideoContent.ThumbnailUrl,

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



    }
}
