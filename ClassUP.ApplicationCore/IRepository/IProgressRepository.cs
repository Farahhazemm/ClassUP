using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IProgressRepository : IBaseRepository<LectureProgress>
    {
        Task<LectureProgress> GetByEnrollmentAndLectureAsync(int enrollmentId, int lectureId);


    }
}
