using System;
using System.Collections.Generic;
using System.Text;

namespace ClassUP.ApplicationCore.DTOs.Responses.Lectures
{
    public class LectureDetailDto
    {
        #region Main Prop
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Duration { get; set; }
        public int OrderIndex { get; set; }
        public int SectionId { get; set; }
        public bool IsFree { get; set; } 
        #endregion

        #region OP :Video or Article details

        public VideoResult? VideoContent { get; set; }
        public ArticleContentDTO? ArticleContent { get; set; }
        #endregion

        #region Progress info
       
        public List<LectureProgressDTO> LectureProgresses { get; set; } = new List<LectureProgressDTO>(); 
        #endregion

    }
}
