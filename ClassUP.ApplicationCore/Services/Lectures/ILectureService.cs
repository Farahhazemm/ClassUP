using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Requests.Lectures;
using ClassUP.ApplicationCore.DTOs.Responses;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Lectures
{
    public interface ILectureService
    {
        Task<IEnumerable<LectureDto>> GetLecturesAsync(FilterOptions filterOptions );
        Task<LectureDetailDto?> GetByIdAsync(int id);
        Task<LectureDto> AddAsync( CreateLectureRequest request);
        Task UploadLectureVideoAsync(int lectureId, IFormFile file);
        Task DeleteLectureVideoAsync(int lectureId);


    }
}
