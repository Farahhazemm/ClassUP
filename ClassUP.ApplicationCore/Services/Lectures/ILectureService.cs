using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Lectures
{
    public interface ILectureService
    {
        Task<IEnumerable<LectureDto>> GetLecturesAsync(FilterOptions filterOptions );
    }
}
