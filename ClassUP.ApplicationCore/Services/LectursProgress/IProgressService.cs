using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.Services.LectursProgress
{
    public interface IProgressService
    {
        Task MarkLessonAsCompletedAsync(int lectureId, string userId);
        Task UnCompleteLessonAsync(int lectureId, string userId);
        Task<bool> IsLessonCompletedAsync(int lectureId, string userId);

    }
}
