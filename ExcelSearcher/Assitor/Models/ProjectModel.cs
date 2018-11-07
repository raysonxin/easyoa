using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assitor.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Contact { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StopDate { get; set; }

        public int State { get; set; }
    }
}
