using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class ArticleContent
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public Lecture lecture { get; set; }
        public string Content { get; set; } 
        public int ReadingTime { get; set; } // in minutes
       
    }
}
