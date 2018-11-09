using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assitor.Models
{
    public class ProjectStatModel
    {
        public int ProjId { get; set; }

        public string ProjName { get; set; }

        public string ProjCode { get; set; }

        public string Contact { get; set; }

        public DateTime ProjStartDate { get; set; }

        public DateTime ProjStopDate { get; set; }

        public double Workload { get; set; }

        public List<EmployeeWorkloadModel> EmpWorkload { get; set; }
    }

    public class EmployeeWorkloadModel
    {
        public string DingId { get; set; }

        public string EmpName { get; set; }

        public string EmpJob { get; set; }

        public int DayCount { get; set; }

        public double HourCount { get; set; }

        public double Total { get; set; }

        public List<CheckModel> CheckRecords { get; set; }
    }
}
