using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Category
    {
        #region My properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        #endregion

        #region Navigation properties
        public List<SubCategory> SubCategories { get; set; }
        public List<Course> Courses { get; set; } 
        #endregion

    }
}
