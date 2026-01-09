using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class CourseObjective
    {
        public int Id { get; set; }
        public string ObjectiveText { get; set; }
        public int OrderIndex { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        
    }
}
