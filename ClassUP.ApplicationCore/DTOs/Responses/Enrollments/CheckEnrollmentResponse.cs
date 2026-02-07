using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Enrollments
{
    public class CheckEnrollmentResponse
    {
        public bool IsEnrolled { get; set; }
        public DateTime? EnrollmentDate { get; set; }
    }
}
