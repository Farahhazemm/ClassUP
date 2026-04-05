using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class VideoContent : BaseEntity
    {
        public int LectureId { get; set; }
        public Lecture lecture { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public string PublicId { get; set; } = null!;

    }
}
