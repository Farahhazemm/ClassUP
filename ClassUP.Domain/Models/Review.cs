using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Review : BaseEntity
    {

        public int CourseId { get; set; }
        public string UserId { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; } = null!;

        public AppUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;

    }
}
