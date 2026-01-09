using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class SubCategory
    {
        #region My properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        #endregion

        #region Navigation properties
        public List<CourseSubCategory> CourseSubCategories { get; set; } 
        #endregion
    }
}
