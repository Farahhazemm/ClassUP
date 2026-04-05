using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class ArticleContent : BaseEntity
    {
        public int Id { get; set; }
        public int LectureId { get; set; }
        public Lecture lecture { get; set; } = null!;
        public string Content { get; set; } = string.Empty;

    }
}
