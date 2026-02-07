using ClassUP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.IRepository
{
    public interface IEnrollmentRepository : IBaseRepository<Enrollment>
    {
        Task<Enrollment> GetEnrollmentAsync(int userId, int courseId);
        Task<bool> IsEnrolledAsync(int userId, int courseId);

    }
}
