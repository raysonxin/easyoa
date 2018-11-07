using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assitor.Models
{
    public class ProjectStaffModel
    {
        public int Id { get; set; }

        public string DingId { get; set; }

        public int ProjId { get; set; }

        public DateTime StartDate { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }
    }
}
