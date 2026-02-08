using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Requests.Enrollment
{
    public class CreateEnrollmentRequest
    {
        public int CourseId { get; set; }

        public string StudentId { get; set; }
    }
}
