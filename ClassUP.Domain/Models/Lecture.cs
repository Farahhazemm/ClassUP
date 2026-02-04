using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Lecture
    {
        #region My properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public String Type { get; set; } // Video, Article
        public int SectionId { get; set; }

        public bool IsFree { get; set; } // for Preview lessons
        

        #endregion


        #region Navigation properties
        public Section Section { get; set; }
        public VideoContent? VideoContent { get; set; }
        public ArticleContent? ArticleContent { get; set; }
        public List<LectureProgress> LectureProgresses { get; set; } 
        #endregion
    }
}
