using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Section
    {
        #region My properties
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public int OrderIndex { get; set; }
        #endregion

        #region Navigation properties
       
        public Course Course { get; set; }
        public List<Lecture> Lectures { get; set; } = new();
        #endregion
    }
}
