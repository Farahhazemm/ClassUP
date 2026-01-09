using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class CourseSubCategory  // for many to many 
    {
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
