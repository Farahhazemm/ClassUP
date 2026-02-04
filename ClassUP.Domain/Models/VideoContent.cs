using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class VideoContent
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public Lecture lecture { get; set; }
        public string VideoUrl { get; set; } 

    }
}
