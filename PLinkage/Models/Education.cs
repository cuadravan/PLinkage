using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkage.Models
{
    internal class Education
    {
        public string CourseName { get; set; } = string.Empty;
        public string SchoolAttended { get; set; } = string.Empty;
        public DateTime TimeGraduated { get; set; } = DateTime.Now;
    }
}
