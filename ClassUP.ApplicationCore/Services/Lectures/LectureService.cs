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
