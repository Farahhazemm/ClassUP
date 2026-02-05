using ClassUP.ApplicationCore.Common.Filters;
using ClassUP.ApplicationCore.DTOs.Responses;
using ClassUP.ApplicationCore.DTOs.Responses.Lectures;
using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface ILectureRepository : IBaseRepository<Lecture>
    {
        Task<Lecture?> GetByIdWithDetailsAsync(int id);

        Task<IEnumerable<Lecture>> GetSectionLectursAsync(int sectionId);   

    }
}
