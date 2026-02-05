using ClassUP.ApplicationCore.DTOs.Requests.Reviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.Reviws
{
    public interface IReviewService
    {
        Task AddAsync(CourseReviewDTO reviewDTO);
    }
}
