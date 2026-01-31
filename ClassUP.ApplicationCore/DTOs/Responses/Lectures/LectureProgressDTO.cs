using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Lectures
{
    public class LectureProgressDTO
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchedDuration { get; set; }
        public DateTime LastWatchedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
