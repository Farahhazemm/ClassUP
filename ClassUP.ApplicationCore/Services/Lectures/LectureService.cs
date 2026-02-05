using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses.Cources;
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

        public async Task<IEnumerable<LectureDTO>> GetLecturesAsync(FilterOptions filter)
        {
            var lectures = await _unitOfWork.Lectures.GetAllAsync(filter);

            return lectures.Select(l => new LectureDTO
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                Type = l.Type.ToString(),
                SectionId = l.SectionId,
                IsFree = l.IsFree
            });
        }

        #region GetLecturesBySection
        public async Task<IEnumerable<LectureDTO>> GetBySectionIdAsync(int sectionId)
        {
            var lectures = await _unitOfWork.Lectures.GetSectionLectursAsync(sectionId);
            if (lectures == null || !lectures.Any())
                return Enumerable.Empty<LectureDTO>();

            var lectureDtos = lectures.Select(l => new LectureDTO
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                Type = l.Type.ToString(),
                SectionId = l.SectionId,
                IsFree = l.IsFree
            });

            return lectureDtos;


        }
        #endregion

        #region GetById
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

        #endregion


        #region Add
        public async Task<LectureDTO> AddAsync(CreateLectureRequest request)
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

            return new LectureDTO
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                Type = lecture.Type.ToString(),
                SectionId = lecture.SectionId,
                IsFree = lecture.IsFree
            };
        }

        #endregion

        #region Update
        public async Task UpdateAsync(int lectureId, UpdateLectureRequest request)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(lectureId);
            if (lecture == null)
                return;

            if (!string.IsNullOrWhiteSpace(request.Title))
                lecture.Title = request.Title;

            if (!string.IsNullOrWhiteSpace(request.Description))
                lecture.Description = request.Description;

            if (!string.IsNullOrWhiteSpace(request.Type) &&
                Enum.TryParse<LectureType>(request.Type, true, out var lectureType))
            {
                lecture.Type = lectureType;
            }

            if (request.IsFree.HasValue)
                lecture.IsFree = request.IsFree.Value;

            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(int lectureId)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdWithDetailsAsync(lectureId);

            if (lecture == null || lecture.VideoContent == null)
                return;

            await _videoService.DeleteAsync(lecture.VideoContent.PublicId);
            await _unitOfWork.Lectures.RemoveVideoContent(lecture.VideoContent);


            _unitOfWork.Lectures.DeleteAsync(lecture);

            await _unitOfWork.SaveChangesAsync();
        } 
        #endregion


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
