using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Videos;
using ClassUP.Domain.Enums;
using ClassUP.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Lectures
{
    public class LectureService : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVideoService _videoService;
        public LectureService(IUnitOfWork unitOfWork, IVideoService videoService)
        {
            _unitOfWork = unitOfWork;
            _videoService = videoService;
        }

        public async Task<IEnumerable<LectureDto>> GetLecturesAsync(FilterOptions filter)
        {
            var lectures = await _unitOfWork.Lectures.GetAllAsync(filter);

            return lectures.Select(l => new LectureDto
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                Type = l.Type.ToString(),
                SectionId = l.SectionId,
                IsFree = l.IsFree
            });
        }

        public async Task<LectureDetailDto> GetByIdAsync(int id)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdWithDetailsAsync(id)
                ?? throw new KeyNotFoundException("Lecture not found");

            return new LectureDetailDto
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                Type = lecture.Type.ToString(),
                SectionId = lecture.SectionId,
                IsFree = lecture.IsFree,

                VideoContent = lecture.Type == LectureType.Video
                    ? lecture.VideoContent == null ? null : new VideoResult
                    {
                        VideoUrl = lecture.VideoContent.VideoUrl,
                    }
                    : null,

                ArticleContent = lecture.Type == LectureType.Article
                    ? lecture.ArticleContent == null ? null : new ArticleContentDTO
                    {
                        Id = lecture.ArticleContent.Id,
                        ArticleText = lecture.ArticleContent.Content
                    }
                    : null
            };
        }

        public async Task<LectureDto> AddAsync(CreateLectureRequest request)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(request.SectionId)
                ?? throw new ArgumentException("Section not found");

            if (request.Type == LectureType.Article &&
                string.IsNullOrWhiteSpace(request.ArticleContent))
                throw new ValidationException("ArticleContent is required");

            var lecture = new Lecture
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                IsFree = request.IsFree,
                SectionId = request.SectionId,
                ArticleContent = request.Type == LectureType.Article
                    ? new ArticleContent { Content = request.ArticleContent! }
                    : null
            };

            await _unitOfWork.Lectures.AddAsync(lecture);
            await _unitOfWork.SaveChangesAsync();

            return new LectureDto
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                Type = lecture.Type.ToString(),
                SectionId = lecture.SectionId,
                IsFree = lecture.IsFree
            };
        }

        #region UploadVideo
        public async Task UploadLectureVideoAsync(int lectureId, IFormFile file)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(lectureId)
                ?? throw new KeyNotFoundException("Lecture not found");

            if (lecture.Type != Domain.Enums.LectureType.Video)
                throw new ValidationException("Lecture is not video type");

            var uploadResult = await _videoService.UploadAsync(file);

            lecture.VideoContent = new VideoContent
            {
                VideoUrl = uploadResult.VideoUrl,
                PublicId = uploadResult.PublicId
            };

            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region DeleteVideo
        public async Task DeleteLectureVideoAsync(int lectureId)
        {

            var lecture = await _unitOfWork.Lectures.GetByIdWithDetailsAsync(lectureId)
             ?? throw new KeyNotFoundException("Lecture not found");



            if (lecture.Type != LectureType.Video)
                throw new ValidationException("Lecture is not video type");


            if (lecture.VideoContent == null)
                throw new InvalidOperationException("Lecture does not have a video");


            await _videoService.DeleteAsync(lecture.VideoContent.PublicId);


            lecture.VideoContent = null;


            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

    }

}
