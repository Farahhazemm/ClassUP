using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Domain.Models
{
    public class Category : BaseEntity
    {
        #region My properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        #endregion

        #region Navigation properties
        public List<Course>? Courses { get; set; }
        #endregion

    }
}
